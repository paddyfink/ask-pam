using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations.Dtos;
using AskPam.Crm.Organizations;
using AskPam.Domain.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Configuration;
using AskPam.Crm.Controllers.Webhooks.Smooch;
using AskPam.Crm.Conversations;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.WebHooks
{
    [Route("api/[controller]/[action]")]
    [EnableCors("CorsPolicy")]
    public class SmoochController : Controller
    {
        private readonly IConversationsManager _conversationManager;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IRepository<Conversation> _conversationsRepository;
        private readonly IRepository<Conversations.Message> _messageRepository;
        private readonly IRepository<DeliveryStatus, long> _deliveryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Setting, long> _settingRepository;

        public SmoochController(IRepository<Organization, Guid> organizationRepository,
            IRepository<Conversation> conversationsRepository,
            IRepository<Conversations.Message> messageRepository,
            IRepository<Contact> contactRepository,
            IConversationsManager conversationManager,
            IRepository<DeliveryStatus, long> deliveryRepository,
            IUnitOfWork unitOfWork,
            IRepository<Setting, long> settingRepository)
        {
            _organizationRepository = organizationRepository;
            _conversationsRepository = conversationsRepository;
            _messageRepository = messageRepository;
            _conversationManager = conversationManager;
            _deliveryRepository = deliveryRepository;
            _unitOfWork = unitOfWork;
            _settingRepository = settingRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Income([FromBody]JObject request)
        {
            var trigger = (string)request["trigger"];

            switch (trigger)
            {
                case "message:appUser":
                    var payload = request.ToObject<AppUserMessage>();
                    await ProcessUserMessage(payload);
                    break;
                case "delivery:failure":
                    await ProcessDeliveryFailure(request.ToObject<DeliveryFailure>());
                    break;
                case "delivery:success":
                    await ProcessDeliverySuccess(request.ToObject<DeliverySuccess>());
                    break;
                case "conversation:read":
                    await ProcessConversationRead(request.ToObject<ConversationRead>());
                    break;
            }
            return Ok();
        }


        #region Private method

        private async Task ProcessDeliveryFailure(DeliveryFailure payload)
        {
            foreach (var appMsg in payload.messages)
            {
                var message = await _messageRepository.GetAll()
                    .Include(m => m.DeliveryStatus)
                    .Where(m => m.SmoochMessageId == appMsg._id)
                    .FirstOrDefaultAsync();

                if (message != null)
                {
                    var deliv = new DeliveryStatus
                    {
                        MessageId = message.Id,
                        ErrorCode = payload.error.code,
                        ErrorMessage = payload.error.underlyingError?.message,
                        ChannelType = GetChannel(payload.destination.type),
                        Success = false
                    };
                    if (!message.DeliveryStatus.Any(d => d.MessageId == deliv.MessageId && d.ChannelType == deliv.ChannelType))
                        await _deliveryRepository.InsertAsync(deliv);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task ProcessDeliverySuccess(DeliverySuccess payload)
        {
            foreach (var appMsg in payload.messages)
            {
                var message = await _messageRepository.GetAll()
                      .Include(m => m.DeliveryStatus)
                      .Where(m => m.SmoochMessageId == appMsg._id)
                      .FirstOrDefaultAsync();

                if (message != null)
                {
                    var deliv = new DeliveryStatus
                    {
                        MessageId = message.Id,
                        ChannelType = GetChannel(payload.destination.type),
                        Success = true
                    };
                    if (!message.DeliveryStatus.Any(d => d.MessageId == deliv.MessageId && d.ChannelType == deliv.ChannelType))
                        await _deliveryRepository.InsertAsync(deliv);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task ProcessConversationRead(ConversationRead payload)
        {
            if (payload.source.type == "web")
            {
                var conversation = await _conversationsRepository.GetAll().Where(c => c.Contact.SmoochUserId == payload.appUser._id).Select(c => c).FirstOrDefaultAsync();

                if (conversation != null)
                {
                    var messages = await _messageRepository.GetAll()
                        .Where(m => m.ConversationId == conversation.Id)
                        .Where(m => m.MessageStatusId == MessageStatus.Sent.Value)
                        .Where(m => m.DeliveryStatus.All(d => d.ChannelType != ChannelType.Web))
                        .Select(m => m.Id).ToListAsync();

                    foreach (var messageId in messages)
                    {
                        var deliv = new DeliveryStatus
                        {
                            MessageId = messageId,
                            ChannelType = ChannelType.Web,
                            Success = true
                        };
                        await _deliveryRepository.InsertAsync(deliv);
                    }
                }
            }
        }

        private async Task ProcessUserMessage(AppUserMessage appUserMessage)
        {
            var setting = await _settingRepository.FirstOrDefaultAsync(s =>
                s.Name == AppSettingsNames.Smooch.AppId && s.Value == appUserMessage.app._id && s.OrganizationId != null);

            if (setting != null)
            {
                var organization = await _organizationRepository.FirstOrDefaultAsync(o => o.Id == setting.OrganizationId);
                var smoochUserId = appUserMessage.appUser._id;
                var name = appUserMessage.messages[0].name;

                var conversation = await _conversationsRepository.GetAll()
                    .Include(c => c.Channels)
                    .FirstOrDefaultAsync(c => c.SmoochUserId == smoochUserId);

                if (conversation == null)
                {
                    conversation = new Conversation
                    (
                        organization.Id,
                        name,
                        smoochUserId: smoochUserId,
                        color: await _conversationManager.GetRandomConversationColor(),
                        seen: false
                    );
                    await _conversationsRepository.InsertAsync(conversation);
                    await _unitOfWork.SaveChangesAsync();
                }



                foreach (var appMessge in appUserMessage.messages)
                {
                    var message = ConvertMessage(appMessge);
                    foreach (var client in appUserMessage.appUser.clients)
                    {
                        var channelLastSeen = DateTime.Now;
                        var dateSuccess = DateTime.TryParse(client.lastSeen, out channelLastSeen);

                        //Todo: just keep base url
                        var web = client.info?.currentUrl;

                        var channel = conversation.Channels.FirstOrDefault(c => c.Type == GetChannel(client.platform));
                        if (channel == null)
                            conversation.Channels.Add(new Channel
                            {
                                SmoochId = client.id,
                                Type = GetChannel(client.platform),
                                DisplayName = client.platform == "web" ? web : client.displayName,
                                Active = client.active,
                                Primary = client.primary,
                                AvatarUrl = client.avatarUrl,
                                LastSeen = dateSuccess ? channelLastSeen : new DateTime?()

                            });
                        else
                        {
                            channel.LastSeen = dateSuccess ? channelLastSeen : new DateTime?();
                            channel.Active = client.active;
                            channel.Primary = client.primary;
                        }


                    }
                    await _conversationManager.ProcessNewMessage(message, conversation, organization);
                }
            }
        }

        private Conversations.Message ConvertMessage(Controllers.Webhooks.Smooch.Message appMessage)
        {

            return new Conversations.Message
            {
                SmoochMessageId = appMessage._id,
                Text = appMessage.text,
                Status = MessageStatus.Received,
                Date = DateTime.UtcNow,
                Author = appMessage.name,
                ChannelType = GetChannel(appMessage.source.type),
                MessageType = GetType(),
                Seen = false
            };



            MessageType GetType()
            {
                switch (appMessage.type)
                {
                    case "text":
                        return MessageType.Text;
                    case "list":
                        return MessageType.List;
                    case "image":
                        return MessageType.Image;
                    case "carousel":
                        return MessageType.Carousel;
                    default:
                        return MessageType.Text;
                }
            }

        }

        private static ChannelType GetChannel(string type)
        {
            switch (type)
            {
                case "messenger":
                    return ChannelType.Messenger;
                case "twilio":
                    return ChannelType.Sms;
                case "ios":
                    return ChannelType.iOs;
                case "web":
                    return ChannelType.Web;
                case "android":
                    return ChannelType.Android;
                case "viber":
                    return ChannelType.Viber;
                case "telegram":
                    return ChannelType.Telegram;
                case "wechat":
                    return ChannelType.WeChat;
                case "line":
                    return ChannelType.Line;
                case "frontendEmail":
                    return ChannelType.Email;
                default:
                    return ChannelType.Web;
            }
        }
        #endregion
    }
}
