using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Crm.Followers;
using AskPam.Events;
using AskPam.Crm.Organizations;

namespace AskPam.Crm.Notifications
{
    public partial class NotificationHandler
    {
        private INotificationManager _notificationManager;
        private IUserManager _userManager;
        private IFollowersManager _followerManager;
        private IPostmarkService _emailService;
        private IDomainEvents _domainEvents;
        private IOrganizationManager _organizationManager;

        public NotificationHandler(
            INotificationManager notificationManager,
            IUserManager userManager,
            IFollowersManager followerManager,
            IPostmarkService emailService,
            IDomainEvents eventer,
            IOrganizationManager organizationManager
        )
        {
            _notificationManager = notificationManager;
            _userManager = userManager;
            _followerManager = followerManager;
            _emailService = emailService;
            _domainEvents = eventer;
            _organizationManager = organizationManager;
        }
    }
}
