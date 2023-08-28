using AskPam.Crm.Authorization;
using AskPam.Exceptions;
using AskPam.Crm.Runtime.Session;
using AskPam.Crm.Settings;
using AskPam.Extensions;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AskPam.Crm.Helpers.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private ILogger<ApiExceptionFilter> _Logger;
        private ICrmSession _session;
        private IUserManager _userManager;
        private ElmahSettings _elmahSettings;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, ICrmSession session, IUserManager userManager, IOptions<ElmahSettings> options)
        {
            _Logger = logger;
            _session = session;
            _userManager = userManager;
            _elmahSettings = options.Value;
        }

        public override void OnException(ExceptionContext context)
        {
            var elmahSessings = new ElmahIoSettings();


            if (!_session.UserId.IsNullOrEmpty())
            {
                var user = _userManager.FindByIdAsync(_session.UserId).Result;
                elmahSessings.OnMessage = (msg) =>
                {
                    if (msg.Data == null)
                        msg.Data = new List<Elmah.Io.Client.Models.Item>();
                    msg.User = user.Id;
                    msg.Data.Add(new Elmah.Io.Client.Models.Item { Key = "X-ELMAHIO-USEREMAIL", Value = user.Email });
                    msg.Data.Add(new Elmah.Io.Client.Models.Item { Key = "X-ELMAHIO-USERNAME", Value = user.FullName });
                    //message.Data.Add(new Elmah.Io.Client.Models.Item { Key = "OrganizationId", Value = _session.OrganizationId });
                };
            }

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            String message = String.Empty;

            List<string> errors = null;

            if (context.Exception is BadRequestException badRequestApiException)
            {
                context.Exception = null;
                message = badRequestApiException.Message;
                statusCode = badRequestApiException.StatusCode;
                errors = badRequestApiException.Errors;

                if (!_elmahSettings.AppId.IsNullOrEmpty())
                {
                    elmahSessings.HandledStatusCodesToLog.Add((int)badRequestApiException.StatusCode);
                    badRequestApiException.Ship(_elmahSettings.AppId, new Guid(_elmahSettings.LogId), context.HttpContext, elmahSessings);
                }

            }
            else if (context.Exception is ApiException apiException)
            {
                context.Exception = null;
                // handle explicit 'known' API errors

                message = apiException.Message;

                statusCode = apiException.StatusCode;

                _Logger.LogWarning($"Application thrown error: {apiException.Message}", apiException);

                if (!_elmahSettings.AppId.IsNullOrEmpty())
                {
                    elmahSessings.HandledStatusCodesToLog.Add((int)apiException.StatusCode);
                    apiException.Ship(_elmahSettings.AppId, new Guid(_elmahSettings.LogId), context.HttpContext, elmahSessings);
                }
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                message = "Unauthorized Access";
                statusCode = HttpStatusCode.Unauthorized;

                _Logger.LogWarning("Unauthorized Access in Controller Filter.");
                if (!_elmahSettings.AppId.IsNullOrEmpty())
                {
                    elmahSessings.HandledStatusCodesToLog.Add(401);
                    context.Exception.Ship(_elmahSettings.AppId, new Guid(_elmahSettings.LogId), context.HttpContext, elmahSessings);
                }
            }
            else
            {
                // Unhandled errors
#if !DEBUG
                var msg = "An unhandled error occurred.";                
                string stack = null;
#else
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
#endif

                message = msg;
                statusCode = HttpStatusCode.InternalServerError;

                _Logger.LogError(context.Exception, msg);

                if (!_elmahSettings.AppId.IsNullOrEmpty() && context.HttpContext.Response.StatusCode != 404)
                {
                    elmahSessings.HandledStatusCodesToLog.Add(500);
                    context.Exception.Ship(_elmahSettings.AppId, new Guid(_elmahSettings.LogId), context.HttpContext, elmahSessings);
                }
            }

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;

            response.ContentType = "application/json";

            var apiError = new ApiError(message);

            context.Result = new JsonResult(apiError);
        }
    }
}
