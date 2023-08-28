using System;
using AskPam.Crm.AI;
using AskPam.Crm.Conversations.Events;
using AskPam.Events;
using System.Threading.Tasks;
using System.Globalization;
using AskPam.Domain.Repositories;
using AskPam.Crm.Configuration;
using AskPam.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AskPam.Crm.Notifications;
using Newtonsoft.Json;
using AskPam.Crm.Followers;
using AskPam.Extensions;

namespace AskPam.Crm.Conversations
{
    public class ConversationEventHandler :
        IEventHandler<MessageReceivedEvent>,
        IEventHandler<ConversationReadEvent>,
        IEventHandler<MessageSentEvent>

    {
        private readonly IConversationsManager _conversationManager;
        private readonly IFollowersManager _followersManager;

        public ConversationEventHandler(IConversationsManager conversationManager,
            IFollowersManager followersManager)
        {
            _conversationManager = conversationManager;
            _followersManager = followersManager;
        }

        public async Task Handle(MessageReceivedEvent notification)
        {
            await _conversationManager.PostBotMessageAsync(notification.Conversation, notification.Message);
            //TextAnalysis(eventData.Message);
        }

        public async Task Handle(ConversationReadEvent args)
        {
            await _conversationManager.MarkAsRead(args.Conversation.Id);
        }

        public async Task Handle(MessageSentEvent args)
        {
            if (args.Conversation.AssignedToId.IsNullOrEmpty())
            {
                await _conversationManager.AssigntoUser(args.Conversation, args.User);
            }

            if (! await _followersManager.IsFollowing(args.User, args.Conversation))
            {
                await _followersManager.Follow(args.User, args.Conversation, false);
            }
        }
    }
}
