using AskPam.Crm.Authorization;
using AskPam.Crm.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Crm
{
    public interface IMailService
    {
        Task SendNotificationAsync(string to, string subject, string body, string entity, string link);
    }
}
