using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Crm.Organizations;
using System;
using System.Threading.Tasks;

namespace AskPam.Crm.Storage
{
    public interface IStorageService
    {
        Task<string> AddFile(byte[] file, string fileName, Conversation Conv);
        Task<string> SaveProfilePicture(byte[] file, string fileName, User user);
        Task RemoveProfilePicture(User user);
    }
}
