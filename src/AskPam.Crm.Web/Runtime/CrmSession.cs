using AskPam.Crm.Authorization;
using AskPam.Crm.Organizations;
using AskPam.Crm.Runtime.Session;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AskPam.Crm.Runtime
{
    public class CrmSession : ICrmSession
    {
        private readonly IHttpContextAccessor _context;

        public CrmSession(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string UserId
        {
            get
            {
                return _context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            }
        }

        public Guid? OrganizationId
        {
            get
            {
                Guid orgId = Guid.Empty;
                Guid.TryParse(_context.HttpContext?.Request.Headers["Organization"].FirstOrDefault(), out orgId);
                return orgId != Guid.Empty ? new Guid?(orgId) : null;
            }
        }

        public User User
        {
            get
            {
                    return null;
            }
        }

        public Organization Organization
        {
            get
            {
                    return null;
            }
        }
    }
}
