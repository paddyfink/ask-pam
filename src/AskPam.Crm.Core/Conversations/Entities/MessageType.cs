using System;
using System.Collections.Generic;
using System.Text;
using AskPam.Domain.Entities;

namespace AskPam.Crm.Conversations
{
 
    public class MessageType : Enumeration<MessageType, string>
    {
        public static readonly MessageType Text = new MessageType("Text", "Text");
        public static readonly MessageType Image = new MessageType("Image", "Image");
        public static readonly MessageType List = new MessageType("List", "List");
        public static readonly MessageType Carousel = new MessageType("Carousel", "Carousel");
        public static readonly MessageType Notification = new MessageType("Notification", "Notification");
        public static readonly MessageType Warning = new MessageType("Warning", "Warning");
        public static readonly MessageType Note = new MessageType("Note", "Note");

        private MessageType(string value, string displayName) : base(value, displayName) { }
    }

}
