using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Notifications.Dtos
{
    public class GetNotificationsRequestDto
    {
        public bool? Read { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
    }
}
