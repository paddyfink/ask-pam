//using AskPam.Crm.Authorization;
//using AskPam.Crm.Authorization.Organizations;
//using AskPam.Domain.Repositories;
//using Microsoft.AspNetCore.Http;
//using SaasKit.Multitenancy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace AskPam.Crm.Web
//{
//    public class OrganizationResolver : ITenantResolver<Organization>
//    {
//        private IRepository<Organization, Guid> _accountRepository;

//        public OrganizationResolver(IRepository<Organization, Guid> accountRepository)
//        {
//            _accountRepository = accountRepository;
//        }

//        public async Task<TenantContext<Organization>> ResolveAsync(HttpContext context)
//        {
//            TenantContext<Organization> tenantContext = null;
//            var userId = context.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();

//            //if (!string.IsNullOrEmpty(userId))
//            //{
                
//            //    int accountId = 0;
//            //    int.TryParse(context.Request.Headers["Organization"].FirstOrDefault(), out accountId);

//            //    if (accountId > 0)
//            //    {
//            //        var account = await _accountRepository.FirstOrDefaultAsync(a => a.Id == accountId && a.Users.Any(u => u.UserId == userId));

//            //        if (account != null)
//            //        {
//            //            tenantContext = new TenantContext<Organization>(account);
//            //        }
//            //    }
//            //}

//            return tenantContext;
//        }
//    }
//}
