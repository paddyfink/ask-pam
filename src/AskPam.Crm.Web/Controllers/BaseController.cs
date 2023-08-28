using AskPam.Exceptions;
using AskPam.Crm.Runtime.Session;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace AskPam.Crm
{
    public class BaseController : Controller
    {
       public ICrmSession Session { get; }
        public IMapper Mapper { get; }
        public BaseController([FromServices] ICrmSession session, [FromServices] IMapper mapper)
        {
            this.Session = session;
            Mapper = mapper;
        }

        protected void EnsureOrganization()
        {
            if (!Session.OrganizationId.HasValue)
            {
                throw new ApiException("Organization is required", HttpStatusCode.BadRequest);
            }
        }
    }
}
