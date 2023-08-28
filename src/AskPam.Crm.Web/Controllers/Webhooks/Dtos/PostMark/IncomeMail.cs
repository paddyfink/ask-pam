using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations.Dtos
{
    public class IncomeMail
    {
        public string From { get; set; }
        public string FromName { get; set; }
        public FromFull FromFull { get; set; }
        public string To { get; set; }
        public List<ToFull> ToFull { get; set; }
        public string Cc { get; set; }
        public List<CcFull> CcFull { get; set; }
        public string Bcc { get; set; }
        public List<BccFull> BccFull { get; set; }
        public string OriginalRecipient { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string MessageID { get; set; }
        public string Date { get; set; }
        public string MailboxHash { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public string StrippedTextReply { get; set; }
        public string Tag { get; set; }
        public List<Header> Headers { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    public class FromFull
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string MailboxHash { get; set; }
    }

    public class ToFull
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string MailboxHash { get; set; }
    }

    public class CcFull
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string MailboxHash { get; set; }
    }

    public class BccFull
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string MailboxHash { get; set; }
    }

    public class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Attachment
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }
        public string ContentID { get; set; }
    }

}
