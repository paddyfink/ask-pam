using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AskPam.Domain.Services
{
    public abstract class DomainService : ServiceBase, IDomainService
    {
        public DomainService(ILogger logger, IStringLocalizer localizer) : base(logger, localizer)
        {
        }
    }
}
