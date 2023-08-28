using AskPam.Domain.Entities;

namespace AskPam.Crm.Conversations
{
    
    public class ChannelType : Enumeration<ChannelType, string>
    {
        public static readonly ChannelType Empty = new ChannelType("", "");
        public static readonly ChannelType Email = new ChannelType("Email", "Email");
        public static readonly ChannelType Sms = new ChannelType("Sms", "Sms");
        public static readonly ChannelType iOs = new ChannelType("ios", "iOs");
        public static readonly ChannelType Android = new ChannelType("Android", "Android");
        public static readonly ChannelType Web = new ChannelType("Web", "Web");
        public static readonly ChannelType Messenger = new ChannelType("Messenger", "Messenger");
        public static readonly ChannelType Twitter = new ChannelType("Twitter", "Twitter");
        public static readonly ChannelType WeChat = new ChannelType("WeChat", "WeChat");
        public static readonly ChannelType Line = new ChannelType("Line", "Line");
        public static readonly ChannelType Telegram = new ChannelType("Telegram", "Telegram");
        public static readonly ChannelType Viber = new ChannelType("Viber", "Viber");

        private ChannelType(string value, string displayName) : base(value, displayName) { }
    }
}
