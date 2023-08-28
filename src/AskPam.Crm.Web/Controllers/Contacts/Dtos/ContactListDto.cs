using AskPam.Crm.Conversations.Dtos;
using AskPam.Crm.Tags.Dtos;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Contacts.Dtos
{
    public class ContactListDto
    {
        public long Id { get; set; }
        public string FirstName { get; private set; } 
        public string LastName { get; private set; } 
        public string FullName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public string AssignedToFullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsNew { get; set; }
        public int ConversationsCount { get; set; }
        public DateTime? AssignedToDate { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
