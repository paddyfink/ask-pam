using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Conversations
{
    public class MessageStatus : Enumeration<MessageStatus, string>
    {
        public static readonly MessageStatus Received = new MessageStatus("Received", "Received");
        public static readonly MessageStatus Sent = new MessageStatus("Sent", "Sent");

        private MessageStatus(string value, string displayName) : base(value, displayName) { }
    }
}
