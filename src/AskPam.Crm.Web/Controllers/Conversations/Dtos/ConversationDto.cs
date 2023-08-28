using AskPam.Crm.Contacts.Dtos;
using AskPam.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Common;
using AskPam.Crm.Tags.Dtos;

namespace AskPam.Crm.Conversations.Dtos
{
    public class 
        ConversationDto: FullAuditedDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }        
        public string AssignedToId { get; set; }
        public string AssignedToFullName { get; set; }
        public string AssignedToPicture { get; set; }
        public string SmoochUserId { get; set; }
        public string Email { get; set; }
        public string AvatarColor { get; set; }
        public IEnumerable<MessageDto> Messages { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFlagged { get; set; }
        public bool? IsStarred { get; set; }
        public bool? BotDisabled { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public ContactDto Contact { get; set; }
        public IEnumerable<ChannelDto> Channels { get; set; }
        public Geo LastLocation { get; set; }
    }
}
