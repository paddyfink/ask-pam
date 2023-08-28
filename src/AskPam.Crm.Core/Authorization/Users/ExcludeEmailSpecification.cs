using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AskPam.Crm.Configuration;
using AskPam.Domain.Specifications;
using System.Linq;

namespace AskPam.Crm.Authorization.Users
{
    public class ExcludeAdminEmailSpecification : Specification<User>
    {
        private readonly string[] _emailsToExclude;


        public ExcludeAdminEmailSpecification(ISettingManager settingManager)
        {
            _emailsToExclude = settingManager
                .GetSettingValueForApplicationAsync(AppSettingsNames.Application.EmailsToExclude).Result.Split(',');
        }
        public override Expression<Func<User, bool>> ToExpression()
        {
            return (user) => (!_emailsToExclude.Contains(user.Email));
        }
    }
}
