//using AskPam.Crm.Contacts;
//using AskPam.Crm.Conversations;
//using AskPam.Crm.Conversations.Dtos;
//using AskPam.Crm.Conversations.Dtos.Broid;
//using AskPam.Crm.Integrations;
//using AskPam.Crm.Organizations;
//using AskPam.Crm.PhoneNumberLookup;
//using AskPam.Domain.Repositories;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace AskPam.Crm.Conversations
//{
//    [Route("api/[controller]/[action]")]
//    [EnableCors("CorsPolicy")]
//    public class BroidController : Controller
//    {
//        private readonly IConversationManager _conversationManager;
//        private readonly IRepository<Organization, Guid> _organizationRepository;
//        private readonly IRepository<Conversation> _conversationsRepository;
//        private readonly IRepository<Message> _messageRepository;
//        private readonly IRepository<Channel> _channelRepository;
//        private IPhoneManager _phoneManager;
//        IRepository<Integration> _integrationRepository;

//        public BroidController(IRepository<Organization, Guid> organizationRepository,
//            IRepository<Conversation> conversationsRepository,
//            IRepository<Message> messageRepository,
//            IConversationManager conversationManager,
//            IRepository<Channel> channelRepository,
//            IPhoneManager phoneManager,
//            IRepository<Integration> integrationRepository)
//        {
//            _organizationRepository = organizationRepository;
//            _conversationsRepository = conversationsRepository;
//            _messageRepository = messageRepository;
//            _conversationManager = conversationManager;
//            _channelRepository = channelRepository;
//            _phoneManager = phoneManager;
//            _integrationRepository = integrationRepository;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Income([FromBody]JObject request)
//        {
//            var messageActivity = request.ToObject<MessageActivity>();

//            var integration = await _integrationRepository.FirstOrDefaultAsync(i => i.BroidId == messageActivity.message.generator.id);

//            if (integration != null)
//            {
//                var organization = await _organizationRepository.GetAsync(integration.OrganizationId);

//                var message = new Message
//                {

//                    Text = messageActivity.message.@object.content,
//                    Status = MessageStatus.Received,
//                    Date = DateTime.UtcNow,
//                    Name = messageActivity.message.actor.name,
//                    Channel = GetChannel(messageActivity.message.generator.name),
//                    Type = getType(messageActivity.message.@object.type),
//                    Seen = false
//                };

//                var channel = _channelRepository.GetAll()
//                       .Include(c => c.Contact)
//                       .Where(c => c.OrganizationId == organization.Id && c.Type == message.Channel && c.Recipient == messageActivity.message.actor.id)
//                       .FirstOrDefault();

//                if (channel == null)
//                {
//                    channel = new Channel
//                    {
//                        Type = message.ChannelType,
//                        Recipient = messageActivity.message.actor.id,
//                        DisplayName = messageActivity.message.actor.name,
//                        Active = true,
//                        OrganizationId = organization.Id,
//                    };
//                }

//                await _conversationManager.ProcessNewMessage(message, channel, organization);
//            }
//            return Ok();
//        }


//        #region Private method



//        private ChannelType GetChannel(string type)
//        {
//            switch (type)
//            {
//                case "messenger":
//                    return ChannelType.Facebook;
//                case "twilio":
//                    return ChannelType.Sms;
//                case "ios":
//                    return ChannelType.iOs;
//                case "web":
//                    return ChannelType.Web;
//                case "android":
//                    return ChannelType.Android;
//                case "viber":
//                    return ChannelType.Viber;
//                case "telegram":
//                    return ChannelType.Telegram;
//                case "wechat":
//                    return ChannelType.WeChat;
//                case "line":
//                    return ChannelType.Line;
//                default:
//                    return ChannelType.Web;
//            }
//        }

//        private MessageType getType(string type)
//        {
//            switch (type)
//            {
//                case "Note":
//                    return MessageType.Text;
//                case "Image":
//                    return MessageType.Image;
//                case "Document":
//                    return MessageType.Document;
//                case "Collection":
//                    return MessageType.Carousel;
//                default:
//                    return MessageType.Text;
//            }
//        }
//        #endregion
//    }
//}
