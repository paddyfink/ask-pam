using AskPam.Crm.Conversations.Dtos;
using AskPam.Crm.Conversations.Events;
using AskPam.Events;
using AskPam.Crm.Hubs;
using AutoMapper;
using System.Linq;
using System;
using AskPam.Crm.Contacts.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AskPam.Crm.Conversations
{
    //[Authorize]
    public class ConversationHubHandler : IEventHandler<MessageReceivedEvent>,
        IEventHandler<MessageSentEvent>,
        IEventHandler<ConversationReadEvent>,
        IEventHandler<ConversationFlaggedEvent>,
        IEventHandler<ConversationArchivedEvent>
    {
        private readonly IHubContext<ConversationHub> _connectionManager;
        private readonly IMapper _mapper;

        public ConversationHubHandler(IHubContext<ConversationHub> connectionManager, IMapper mapper) : base()
        {
            this._connectionManager = connectionManager;
            this._mapper = mapper;
        }

        public async Task Handle(MessageReceivedEvent @event)
        {
            await NotifyNewMessage(@event.Conversation, @event.Message);
        }

        public async Task Handle(MessageSentEvent @event)
        {
            await NotifyNewMessage(@event.Conversation, @event.Message);
        }

        public async Task Handle(ConversationArchivedEvent @event)
        {
            var conversation = @event.Conversation;

            await _connectionManager.Clients.Group(conversation.OrganizationId.ToString()).InvokeAsync("conversationArchived", conversation.Id, @event.User.Id);
        }

        public async Task Handle(ConversationFlaggedEvent args)
        {
            var conversation = args.Conversation;

           await _connectionManager.Clients.Group(conversation.OrganizationId.ToString()).InvokeAsync("conversationFlagged", conversation.Id, args.User.Id);
        }           

        public async Task Handle(ConversationReadEvent args)
        {
           await _connectionManager.Clients.Group(args.Conversation.OrganizationId.ToString())
               .InvokeAsync("conversationRead", args.Conversation.Id, args.User.Id);
        }

        public async Task NotifyNewMessage(Conversation conversation, Message message)
        {
            var msgDto = _mapper.Map<Message, MessageDto>(message);
            var convDto = _mapper.Map<Conversation, ConversationListDto>(conversation);
            convDto.LastMessage = msgDto;
                
            //var dconvDto = new ConversationListDto
            //{
            //    Id = conversation.Id,
            //    Name = conversation.Contact!=null? conversation.Contact.FullName: conversation.Name,
            //    OrganizationId= conversation.OrganizationId,
            //    Contact = conversation.Contact != null ? new SimpleContactDto { Id = conversation.Contact.Id, FullName = conversation.Contact.FullName, EmailAddress = conversation.Contact.EmailAddress } : null,
            //    AvatarColor = conversation.AvatarColor,
            //    AssignedToId = conversation.AssignedToId,
            //    IsFlagged = conversation.IsFlagged,
            //    Seen = conversation.Seen,
            //    LastMessage = msgDto
            //};
            await _connectionManager.Clients.Group(conversation.OrganizationId.ToString())
                          .InvokeAsync("newMessage", convDto);
        }
    }
}
