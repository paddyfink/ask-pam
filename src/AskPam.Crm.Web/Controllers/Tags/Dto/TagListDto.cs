using System;

namespace AskPam.Crm.Tags.Dto
{
    public class TagListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string FullName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ContactsCount { get; set; }
        public int LibraryItemsCount { get; set; }
        public int ConversationsCount { get;  set; }
    }
}
