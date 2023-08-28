using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;

namespace AskPam
{
    public abstract class ServiceBase
    {
        private readonly ILogger _logger;
        private readonly IStringLocalizer _localizer;

        public ServiceBase(ILogger logger, IStringLocalizer localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }
    }
}
