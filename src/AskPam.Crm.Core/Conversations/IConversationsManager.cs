using System;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Authorization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Organizations;
using AskPam.Domain.Services;

namespace AskPam.Crm.Conversations
{
    public interface IConversationsManager: IDomainService
    {
        Task ActivateBot(Conversation conversation);
        Task<Message> AddNote(Conversation conversation, User user, Message note);
        Task Archive(Conversation conversation, User user);
        Task AssigntoUser(Conversation conversation, User assignee, User assigner = null);
        Task<Conversation> FindByIdAsync(long id, Guid organizationId);
        Task Flag(Conversation conversation, User user = null);
        Task<string> GetRandomConversationColor();
        Task<Message> ProcessNewMessage(Message message, Conversation conversation, Organization organization);
        Task MarkAsRead(long conversationId);
        Task MarkAsUnRead(long conversationId);
        Task RemoveUserAssignment(Conversation conversation);
        Task<Message> SendMessage(Conversation conversation, Message message, User user);
        Task PostBotMessageAsync(Conversation conversation, Message message);
        Task LinkContact(Conversation conversation, Contact contact);
        Task UnlinkContact(Conversation conversation);
    }
}