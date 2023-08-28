using AskPam.Crm.Contacts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Tags.Dtos;

namespace AskPam.Crm.Conversations.Dtos
{
    public class ConversationListDto
    {
        public long Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
        public bool Seen { get; set; }
        public MessageDto LastMessage { get; set; }
        public string AssignedToId { get; set; }
        public string AssignedToFullName { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsStarred { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? Date => LastMessage?.Date ?? CreatedAt;

        public string AvatarColor { get; set; }
        public bool? IsActive { get; set; }
        public bool? BotDisabled { get; set; }
        public SimpleContactDto Contact { get; set; }
        public IEnumerable<ChannelDto> Channels { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}
