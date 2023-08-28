using AskPam.Crm.Organizations;
using System.Threading.Tasks;
using AskPam.Crm.Contacts;

namespace AskPam.Crm.Conversations
{
    public interface IPostmarkService
    {
        Task<string> SendEmailAsync(Organization org,  Email mail);
        Task<int> AddEmailSenderAsync(string email, string name);
        Task<int> UpdateEmailSenderAsync(int signatureSenderId, string name);
        Task DeleteEmailSenderAsync(int signatureSenderId);
    }
}