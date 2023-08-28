using AskPam.Crm.Conversations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Smooch.Model
{
    public class Conversation
    {
        public int unreadCount { get; set; }
        public string _id { get; set; }
    }

    public class Source
    {
        public string type { get; set; }
    }

    public class Message
    {
        public string name { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string role { get; set; }
        public double received { get; set; }
        public string authorId { get; set; }
        public string avatarUrl { get; set; }
        public string _id { get; set; }
    }

    public class AppMakerMessage
    {
        public Conversation conversation { get; set; }
        public Message message { get; set; }
    }

   
}
