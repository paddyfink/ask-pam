using AskPam.Crm.AI;
using AskPam.Crm.Authorization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations.Events;
using AskPam.Events;
using AskPam.Exceptions;
using AskPam.Crm.Storage;
using AskPam.Domain.Repositories;
using AskPam.Domain.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AskPam.Crm.Settings;
using Microsoft.Extensions.Options;
using AskPam.Crm.Organizations;
using System.Text;
using AskPam.Extensions;
using AskPam.Crm.Configuration;
using AskPam.Crm.Helpers;
using Z.EntityFramework.Plus;
using AskPam.EntityFramework.Repositories;
using System.Net;

namespace AskPam.Crm.Conversations
{
    public class ConversationsManager : IConversationsManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Conversation> _conversationsRepository;
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IDomainEvents _domainEvents;
        private readonly IStorageService _storageService;
        private readonly IPostmarkService _postmarkEMailService;
        private readonly ISettingManager _settingManager;
        private readonly IQnAMakerManager _qnAMakerManager;
        private readonly ISmoochConversationService _smoochService;


        public ConversationsManager(
            IUnitOfWork unitOfWork,
            IDomainEvents domainEvents,
            IStorageService storageService,
            IPostmarkService postmarkEMailService,
            ISettingManager settingManager,
            IQnAMakerManager qnAMakerManager,
            ISmoochConversationService smoochService,
            IRepository<Message> messageRepository,
            IRepository<Conversation> conversationsRepository,
            IRepository<Contact> contactsRepository,
            IRepository<Organization, Guid> organizationRepository)
        {
            _unitOfWork = unitOfWork;

            _domainEvents = domainEvents;
            _storageService = storageService;
            _postmarkEMailService = postmarkEMailService;
            _smoochService = smoochService;
            _messageRepository = messageRepository;
            _conversationsRepository = conversationsRepository;
            _organizationRepository = organizationRepository;
            _settingManager = settingManager;
            _contactRepository = contactsRepository;
            _qnAMakerManager = qnAMakerManager;
            //_logger = logger;
        }


        public async Task<string> GetRandomConversationColor()
        {
            var avatarColor = await
                _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Application.AvatarColors);
            try
            {
                var colors = avatarColor.Split(',');
                Random rnd = new Random();
                int index = rnd.Next(colors.Length);
                return colors[index];
            }
            catch (Exception)
            {
                //_logger.LogError(e.Message);
                return string.Empty;
            }
        }

        public async Task<Message> ProcessNewMessage(Message message, Conversation conversation, Organization organization)
        {
            bool newConveration = false;

            if (conversation.ContactId == null && !conversation.Email.IsNullOrEmpty())
            {

                var contact = await _contactRepository
                    .GetAll()
                    .Where(c => c.OrganizationId == organization.Id)
                    .FirstOrDefaultAsync(c =>
                    EF.Functions.Like(c.EmailAddress, conversation.Email) ||
                    EF.Functions.Like(c.EmailAddress2, conversation.Email));

                if (contact != null)
                    conversation.Contact = contact;
            }


            if (message is Email && message.Attachments != null)
                foreach (var attachment in message.Attachments)
                {
                    var url = await SaveAttachmentToCloud(conversation, attachment);
                    attachment.UpdateUrl(url);
                }

            conversation.AddMessage(message);
            conversation.MarkAsUnSeen();

            await _conversationsRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();

            await _domainEvents.RaiseAsync(new MessageReceivedEvent() { Conversation = conversation, Message = message });

            if (newConveration)
                await _domainEvents.RaiseAsync(new ConversationCreatedEvent() { Conversation = conversation, Message = message });

            return message;
        }


        public async Task<Message> SendMessage(Conversation conversation, Message message, User user)
        {


            if (message.MessageType == MessageType.Note)
            {
                throw new ArgumentException("Can't send a note", nameof(message));
            }

            if (message.Attachments != null)
            {
                foreach (var attachment in message.Attachments)
                {
                    attachment.UpdateUrl(await SaveAttachmentToCloud(conversation, attachment));
                }
            }


            if (!conversation.SmoochUserId.IsNullOrEmpty())
            {
                var text = new StringBuilder(message.Text);
                text.AppendLine(string.Empty);

                if (message.Attachments != null)
                    foreach (var attachment in message?.Attachments)
                    {
                        text.AppendLine(attachment.Url);
                    }

                await _smoochService.PostMessageAsync(conversation, message);

            }
            else if (!conversation.Email.IsNullOrEmpty())
            {
                var lastEmail = await _messageRepository.GetAll()
                   .Where(m => m.ConversationId == conversation.Id && m.Status == MessageStatus.Received && m is Email)
                   .OrderByDescending(m => m.Date)
                   .FirstOrDefaultAsync();

                var replyBody = new StringBuilder(StringHelper.TextToHtml(message.Text));

                if (!string.IsNullOrEmpty(user.Signature))
                {
                    replyBody.AppendLine("<br/>");
                    replyBody.AppendLine("<br/>");

                    replyBody.AppendLine(user.Signature);
                }

                if (lastEmail != null)
                {
                    replyBody.AppendLine("<br/>");
                    replyBody.AppendLine("<hr />");
                    replyBody.AppendLine("<p><strong>From</strong>: " + lastEmail.Author + " [mailto:" + (lastEmail as Email)?.From + "]<br/>");
                    replyBody.AppendLine("<strong>Sent</strong>: " + lastEmail.Date.ToString("D") + "<br/>");
                    replyBody.AppendLine("<strong>To</strong>: " + (lastEmail as Email)?.To + "<br />");
                    replyBody.AppendLine("<strong>Subject</strong>: " + (lastEmail as Email)?.Subject + "</p>");
                    replyBody.AppendLine((lastEmail as Email)?.HtmlBody);
                }


                //var lastEmail = (Email)lastMessage;
                var org = await _organizationRepository.FirstOrDefaultAsync(conversation.OrganizationId);
                var organizationEmail = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.SenderEmail, org.Id);

                var postmarkId = new Guid(await _postmarkEMailService.SendEmailAsync(org,
                    new Email()
                    {
                        Author = user.FullName,
                        To = conversation.Email,
                        Cc = (message as Email)?.Cc,
                        Bcc = (message as Email)?.Bcc,
                        From = organizationEmail,
                        HtmlBody = replyBody.ToString(),
                        Subject = !message.Subject.IsNullOrEmpty() ? message.Subject : lastEmail != null ? $"RE: {(lastEmail as Email)?.Subject}" : "",
                        Attachments = message.Attachments
                    }
                ));
                message.PostmarkId = postmarkId;
            }
            conversation.AddMessage(message);
            await _conversationsRepository.UpdateAsync(conversation);

            await _unitOfWork.SaveChangesAsync();

            await _domainEvents.RaiseAsync(new MessageSentEvent { Conversation = conversation, Message = message, User = user });

            return message;
        }

        public async Task<Message> AddNote(Conversation conversation, User user, Message note)
        {
            var org = await _organizationRepository.GetAsync(conversation.OrganizationId);

            conversation.AddMessage(note);
            await _conversationsRepository.UpdateAsync(conversation);

            await _unitOfWork.SaveChangesAsync();

            await _domainEvents.RaiseAsync(new MessageSentEvent { Conversation = conversation, Message = note });

            return note;
        }


        public async Task PostBotMessageAsync(Conversation conversation, Message message)
        {
            bool botHasBeenDeactivated = false;

            if (conversation.BotDisabled || message.ChannelType == ChannelType.Email)
            {
                return;
            }

            var botEnabled = bool.Parse(await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotEnabled, conversation.OrganizationId));
            if (!botEnabled)
            {
                return;
            }

            var knowledgeBaseId = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotKnowledgeBaseId, conversation.OrganizationId);
            if (string.IsNullOrEmpty(knowledgeBaseId))
            {
                return;
            }

            var answer = await _qnAMakerManager.Ask(message.Text, conversation.OrganizationId);
            if (answer == null)
            {
                return;
            }

            var botAvatar = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotAvatar, conversation.OrganizationId);
            var botName = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotName, conversation.OrganizationId);

            var threshold = double.Parse(await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotThreshold, conversation.OrganizationId));

            var isFirstTime = !await _messageRepository.GetAll()
                .Where(m => m.Conversation.OrganizationId == conversation.OrganizationId)
                .AnyAsync(m => m.ConversationId == conversation.Id && m.IsAutomaticReply);


            var score = answer.Score;
            if (score <= threshold)
            {
                var lastMessage = await _messageRepository.GetAll()
                .Where(m => m.Conversation.OrganizationId == conversation.OrganizationId)
                .Where(m => m.ConversationId == conversation.Id && m.MessageStatusId == MessageStatus.Sent.Value).OrderByDescending(m => m.Date).FirstOrDefaultAsync();

                if (lastMessage.IsAutomaticReply)
                {

                    var outro = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotOutro, conversation.OrganizationId);
                    await SendAutomaticReply(outro, conversation.Id);
                    if (!conversation.IsFlagged)
                    {
                        await Flag(conversation);
                    }
                    conversation.ToogleBotActivation();
                    botHasBeenDeactivated = true;

                }
            }
            else
            {
                if (isFirstTime)
                {
                    var intro = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotIntro, conversation.OrganizationId);
                    await SendAutomaticReply(intro, conversation.Id);
                };

                await SendAutomaticReply(answer.Answer, conversation.Id);
            }

            await _conversationsRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();

            if (botHasBeenDeactivated)
                await _domainEvents.RaiseAsync(new ConversationBotAnswerNotFoundEvent
                {
                    Conversation = conversation,
                    Message = message
                });

            async Task SendAutomaticReply(string text, long conversationId)
            {
                var newMessage = new Message
                {
                    ConversationId = conversationId,
                    Text = text,
                    Status = MessageStatus.Sent,
                    Date = DateTime.UtcNow,
                    Author = botName,
                    Avatar = botAvatar,
                    IsAutomaticReply = true,
                    ReplyTo = message.Id,
                    MessageType = MessageType.Text
                };

                await _smoochService.PostMessageAsync(conversation, newMessage);
                newMessage = await _messageRepository.InsertAsync(newMessage);
                await _unitOfWork.SaveChangesAsync();
                await _domainEvents.RaiseAsync(new MessageSentEvent() { Conversation = conversation, Message = newMessage });
            }
        }

        public async Task AssigntoUser(Conversation conversation, User assignee, User assigner = null)
        {
            conversation.AssignToUser(assignee.Id);
            conversation = await _conversationsRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();
            await _domainEvents.RaiseAsync(new ConversationAssignedEvent
            {
                Conversation = conversation,
                Assignee = assignee,
                Assigner = assigner
            });
        }

        public async Task LinkContact(Conversation conversation, Contact contact)
        {
            conversation.AttachToContact(contact.Id);
            await _conversationsRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task UnlinkContact(Conversation conversation)
        {
            conversation.AttachToContact(null);
            await _conversationsRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task RemoveUserAssignment(Conversation conversation)
        {
            conversation.RemoveAssignment();
            conversation = await _conversationsRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Flag(Conversation conversation, User user = null)
        {
            conversation.Flag();
            conversation = await _conversationsRepository.UpdateAsync(conversation);


            await _domainEvents.RaiseAsync(new ConversationFlaggedEvent { Conversation = conversation, User = user });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ActivateBot(Conversation conversation)
        {
            conversation.ToogleBotActivation();

            await _conversationsRepository.UpdateAsync(conversation);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Archive(Conversation conversation, User user)
        {
            var isActive = conversation.IsActive;
            conversation.Active();
            conversation = await _conversationsRepository.UpdateAsync(conversation);
            await _unitOfWork.SaveChangesAsync();

            if (isActive)
            {
                await _domainEvents.RaiseAsync(new ConversationArchivedEvent { Conversation = conversation, User = user });
            }
            else
            {
                await _domainEvents.RaiseAsync(new ConversationUnarchivedEvent { Conversation = conversation, User = user });
            }

        }



        public async Task<Conversation> FindByIdAsync(long id, Guid organizationId)
        {
            var result = await _conversationsRepository.GetAll()
                .Where(c => c.OrganizationId == organizationId)
                .FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (result == null)
            {
                throw new ApiException("Conversation not found, maybe it was deleted.", HttpStatusCode.NotFound);
            }

            return result;
        }

        public async Task MarkAsRead(long conversationId)
        {

            //await _messageRepository.GetAll()
            //    .Where(m => m.ConversationId == conversationId && m.Seen == false && m.MessageStatusId == MessageStatus.Received.Value)
            //    .UpdateAsync(c => new Message { Seen = true });

            var conversation = await _conversationsRepository.FirstOrDefaultAsync(conversationId);
            if (conversation != null)
            {
                conversation.Seen = true;
                await _conversationsRepository.UpdateAsync(conversation);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task MarkAsUnRead(long conversationId)
        {

            await _conversationsRepository.GetAll()
                .Where(c => c.Id == conversationId)
                .UpdateAsync(c => new Conversation { Seen = false });
            await _unitOfWork.SaveChangesAsync();
        }

        #region Private
        private async Task<string> SaveAttachmentToCloud(Conversation conversation, Attachment attachment)
        {
            var byteContent = Convert.FromBase64String(attachment.ContentString);
            return await _storageService.AddFile(byteContent, attachment.Name, conversation);
        }

        #endregion
    }
}
