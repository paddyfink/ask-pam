using AskPam.Crm.Contacts.Dtos;
using AskPam.Crm.Library.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Dashboard.Dtos
{
    public class DashboardDto
    {
        public int ConversationsAssignedCount { get; set; }
        public int ConversationsUnreadCount { get; set; }
        public int ConversationsFlaggedCount { get; set; }
        public int ConversationsFollowedCount { get; set; }

        public int ConversationsCount { get; set; }
        public int MessagesSentCount { get; set; }
        public int MessagesReceivedCount { get; set; }
        public int MessagesUnreadCount { get; set; }
        public IEnumerable<LibraryItemDto> LastLibraryItems { get; set; }
        public IEnumerable<ContactDto> LastContacts { get; set; }
    }
}
