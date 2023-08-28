using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations.Dtos;
using AskPam.Crm.Conversations.Dtos.PostMark;
using AskPam.Crm.Organizations;
using AskPam.Crm.Settings;
using AskPam.Domain.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Common;
using AskPam.Crm.Conversations;
using AskPam.EntityFramework.Repositories;
using Attachment = AskPam.Crm.Conversations.Attachment;

namespace AskPam.Crm.WebHooks
{
    [Route("api/[controller]/[action]")]
    [EnableCors("CorsPolicy")]
    public class PostMarkController : Controller
    {
        private readonly IConversationsManager _conversationManager;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IRepository<Conversation> _conversationsRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<DeliveryStatus, long> _deliveryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PostMarkController(
            IRepository<Organization, Guid> organizationRepository,
            IRepository<Conversation> conversationsRepository,
            IRepository<Message> messageRepository,
            IConversationsManager conversationManager,
            IRepository<DeliveryStatus, long> deliveryRepository,
            IUnitOfWork unitOfWork)
        {
            _organizationRepository = organizationRepository;
            _conversationsRepository = conversationsRepository;
            _messageRepository = messageRepository;
            _conversationManager = conversationManager;
            _deliveryRepository = deliveryRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{organizationId}")]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> Income(Guid organizationId, [FromBody]IncomeMail mail)
        {
            if (mail != null)
            {
                var organization = await _organizationRepository.GetAsync(organizationId);

                if (organization != null)
                {
                    var message = ConvertToEmail(mail);

                    var conversation = await _conversationsRepository.GetAll()
                        .Include(c => c.Contact).FirstOrDefaultAsync(c => c.Email == mail.From.ToLower() && c.OrganizationId == organizationId);

                    if (conversation == null)
                    {
                        conversation = new Conversation
                        (
                            organization.Id,
                            mail.FromName,
                            email: mail.From.ToLower(),
                            color: await _conversationManager.GetRandomConversationColor(),
                            seen: false
                        );

                        conversation.AddChannel(new Channel
                        {
                            DisplayName = mail.From.ToLower(),
                            Active = true,
                            Type = ChannelType.Email
                        });

                        await _conversationsRepository.InsertAsync(conversation);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    await _conversationManager.ProcessNewMessage(message, conversation, organization);
                }

                // If we succesfully received a hook, let the call know
                return Ok();
            }

            // If our message was null, we throw an exception
            return BadRequest();
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> Bounce([FromBody]BouncedMail bouncedMail)
        {
            var messageId = new Guid(bouncedMail.MessageID);

            var message = await _messageRepository.GetAll()
                .Include(m => m.Conversation)
                .Include(m => m.DeliveryStatus)
                .FirstOrDefaultAsync(m => m.PostmarkId == messageId);


            if (message != null)
            {
                var organization = await _organizationRepository.FirstOrDefaultAsync(message.Conversation.OrganizationId);

                StringBuilder text = new StringBuilder();
                text.AppendLine(bouncedMail.Description);
                text.AppendLine("");
                text.AppendLine(bouncedMail.Details);

                Message email = new Message
                {
                    Author = "Ask PAM",
                    Date = DateTime.UtcNow,
                    Text = text.ToString(),
                    Status = MessageStatus.Received,
                    MessageType = MessageType.Warning,
                };

                await _conversationManager.ProcessNewMessage(email, message.Conversation, organization);

                var deliv = new DeliveryStatus
                {
                    MessageId = message.Id,
                    ErrorMessage = text.ToString(),
                    ChannelType = ChannelType.Email,
                    Success = false
                };
                if (!message.DeliveryStatus.Any(d => d.MessageId == deliv.MessageId && d.ChannelType == deliv.ChannelType))
                {
                    await _deliveryRepository.InsertAsync(deliv);
                    await _unitOfWork.SaveChangesAsync();
                }

            }
            return Ok();
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> OpenTrack([FromBody]OpenedEmail openedEmail)
        {
            //if (openedEmail.FirstOpen)
            //{
            var guid = new Guid(openedEmail.MessageID);

            var message = await _messageRepository.GetAll()
                .Include(m => m.Conversation).ThenInclude(m => m.LastLocation)
                .Include(m => m.DeliveryStatus)
                .FirstOrDefaultAsync(m => m.PostmarkId == guid);

            if (message == null) return Ok();

            var deliv = message.DeliveryStatus.FirstOrDefault(d => d.MessageId == message.Id && d.ChannelTypeId == ChannelType.Email.Value);
            if (deliv == null)
            {
                deliv = new DeliveryStatus
                {
                    MessageId = message.Id,
                    ChannelType = ChannelType.Email,
                    Success = true,
                    Open = true,

                };
                await _deliveryRepository.InsertAsync(deliv);
            }
            else
            {
                deliv.Success = true;
                deliv.Open = true;
                await _deliveryRepository.UpdateAsync(deliv);
            }
            if (openedEmail.Geo != null)
            {
                if (message.Conversation.LastLocation.City != openedEmail.Geo.City)
                {
                    var conversation = message.Conversation;
                    float? latitude = null;
                    float? lontitude = null;

                    if (openedEmail.Geo.Coords != null)
                    {
                    var coords = openedEmail.Geo.Coords.Split(',');
                        latitude = float.Parse(coords[0]);
                        lontitude = float.Parse(coords[1]);
                    }
                    conversation.UpdateLastLocation(new Common.Geo
                    {
                        City = openedEmail.Geo.City,
                        Country = openedEmail.Geo.Country,
                        CountryCode = openedEmail.Geo.CountryISOCode,
                        Zip = openedEmail.Geo.Zip,
                        Region = openedEmail.Geo.Region,
                        RegionCode = openedEmail.Geo.RegionISOCode,
                        Ip = openedEmail.Geo.IP,
                        Latitude = latitude,
                        Lontitude = lontitude
                    });
                    await _conversationsRepository.UpdateAsync(conversation);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            //}
            return Ok();
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> Delivery([FromBody]DeliveredEmail openedEmail)
        {
            var messageId = await _messageRepository.GetAll()
                    .Include(m => m.DeliveryStatus)
                    .Where(m => m.PostmarkId == new Guid(openedEmail.MessageID))
                    .Select(m => m.Id)
                    .FirstOrDefaultAsync();

            if (messageId > 0)
            {
                var deliv = new DeliveryStatus
                {
                    MessageId = messageId,
                    ChannelType = ChannelType.Email,
                    Success = true
                };
                //if (!message.DeliveryStatuses.Any(d => d.MessageId == deliv.MessageId && d.Channel == deliv.Channel))
                await _deliveryRepository.InsertAsync(deliv);
                await _unitOfWork.SaveChangesAsync();
            }
            return Ok();
        }

        private Message ConvertToEmail(IncomeMail mail)
        {
            DateTimeOffset offsetDate = DateTimeOffset.Now;

            DateTimeOffset.TryParse(mail.Date, out offsetDate);

            var message = new Email
            {
                Author = mail.FromName,
                From = mail.From,
                To = mail.To,
                Cc = mail.Cc,
                Bcc = mail.Bcc,
                Subject = mail.Subject,
                HtmlBody = mail.HtmlBody,
                TextBody = mail.TextBody,
                Date = offsetDate.UtcDateTime,
                Text = string.IsNullOrEmpty(mail.StrippedTextReply) ? mail.TextBody : mail.StrippedTextReply,
                PostmarkId = new Guid(mail.MessageID),
                Header = string.Join("'", mail.Headers.Select(h => h.Name + ":" + h.Value).ToArray()),
                ChannelType = ChannelType.Email,
                MessageType = MessageType.Text,
                Status = MessageStatus.Received,
                Seen = false
            };

            if (mail.Attachments != null && mail.Attachments.Count > 0)
            {
                message.Attachments = new List<Attachment>();
                foreach (var attachment in mail.Attachments)
                {
                    if (/*string.IsNullOrEmpty(attachment.ContentID) &&*/ attachment.Content != null)
                    {
                        message.AddAttachment(attachment.Name, attachment.Content, attachment.ContentType, attachment.ContentLength);
                    }
                }
            }

            return message;
        }
    }
}
