using System.Collections.Generic;

namespace AskPam.Crm.Controllers.Webhooks.Smooch
{
    public class App
    {
        public string _id { get; set; }
    }

    public class Source
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public class Message
    {
        public string text { get; set; }
        public string role { get; set; }
        public double received { get; set; }
        public string authorId { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string _id { get; set; }
        public Source source { get; set; }
    }

    public class Info
    {
        public string sdkVersion { get; set; }
        public string URL { get; set; }
        public string userAgent { get; set; }
        public string referrer { get; set; }
        public string browserLanguage { get; set; }
        public string currentUrl { get; set; }
        public string currentTitle { get; set; }
    }

    public class Device
    {
        public string lastSeen { get; set; }
        public Info info { get; set; }
        public string platform { get; set; }
        public string id { get; set; }
        public bool active { get; set; }
    }

    public class Destination
    {
        public string type { get; set; }
    }

    public class Client
    {
        public string lastSeen { get; set; }
        public Info info { get; set; }
        public string platform { get; set; }
        public string id { get; set; }
        public bool active { get; set; }
        public bool primary { get; set; }
        public string avatarUrl { get; set; }
        public string displayName { get; set; }
        public string linkedAt { get; set; }
    }

    public class Properties
    {
    }

    public class UnderlyingError
    {
        public string message { get; set; }
    }

    public class Error
    {        public string code { get; set; }
        public UnderlyingError underlyingError { get; set; }
    }

    public class AppUser
    {
        public string _id { get; set; }
        public string userId { get; set; }
        public string signedUpAt { get; set; }
        public bool conversationStarted { get; set; }
        public bool credentialRequired { get; set; }
        public List<Device> devices { get; set; }
        public List<Client> clients { get; set; }
        public List<object> pendingClients { get; set; }
        public Properties properties { get; set; }
    }

    public class AppUserMessage
    {
        public string trigger { get; set; }
        public App app { get; set; }
        public List<Message> messages { get; set; }
        public AppUser appUser { get; set; }
    }

    public class DeliverySuccess
    {
        public string trigger { get; set; }
        public App app { get; set; }
        public AppUser appUser { get; set; }
        public Destination destination { get; set; }
        public List<Message> messages { get; set; }
        public double timestamp { get; set; }
    }

    public class DeliveryFailure
    {
        public string trigger { get; set; }
        public App app { get; set; }
        public AppUser appUser { get; set; }
        public Destination destination { get; set; }
        public Error error { get; set; }
        public List<Message> messages { get; set; }
        public double timestamp { get; set; }
    }

    public class Surviving
    {
        public string _id { get; set; }
        public string userId { get; set; }
        public bool conversationStarted { get; set; }
    }

    public class Discarded
    {
        public string _id { get; set; }
        public bool conversationStarted { get; set; }
    }

    public class MergeAppUser
    {
        public string trigger { get; set; }
        public App app { get; set; }
        public Surviving surviving { get; set; }
        public List<Discarded> discarded { get; set; }
    }

    public class ConversationRead
    {
        public string trigger { get; set; }
        public App app { get; set; }
        public Source source { get; set; }
        public AppUser appUser { get; set; }
        public double timestamp { get; set; }
    }
}
