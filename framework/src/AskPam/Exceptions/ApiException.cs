using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AskPam.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

   

        public ApiException(string message,
                            HttpStatusCode statusCode = HttpStatusCode.InternalServerError) :
            base(message)
        {
            StatusCode = statusCode;
        }
        public ApiException(Exception ex, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(ex.Message)
        {
            StatusCode = statusCode;
        }
    }

    public class ApiError
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public ApiError(string message, List<string> errors = null)
        {
            Message = message;
            if (errors != null)
            {
                Errors = errors;
            }
        }
    }

  


}
