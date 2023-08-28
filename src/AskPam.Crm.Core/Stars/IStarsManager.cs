using System.Threading.Tasks;
using AskPam.Crm.Authorization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Domain.Services;

namespace AskPam.Crm.Stars
{
    public interface IStarsManager : IDomainService
    {
        Task Star(User user, Conversation conversation = null, Contact contact = null);
        Task UnStar(User user, Conversation conversation = null, Contact contact = null);
    }
}