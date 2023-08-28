using AskPam.Crm.Authorization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Domain.Repositories;
using AskPam.Domain.Services;
using System.Threading.Tasks;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.Stars
{
    public class StarsManager : IStarsManager
    {
        private readonly IRepository<StarsRelation, long> _followersRelationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StarsManager(IRepository<StarsRelation, long> followersRelationRepository, IUnitOfWork unitOfWork)
        {
            _followersRelationRepository = followersRelationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Star(User user, Conversation conversation = null, Contact contact = null)
        {
            if (conversation != null)
            {
                await _followersRelationRepository.InsertAsync(
                    new StarsRelation(
                        user.Id,
                       conversationId: conversation.Id
                    )
                );
            }

            if (contact != null)
            {
                await _followersRelationRepository.InsertAsync(
                   new StarsRelation(
                       user.Id,
                      contactId: contact.Id
                   )
               );
            }

            await _unitOfWork.SaveChangesAsync();

        }

        public async Task UnStar(User user, Conversation conversation = null, Contact contact = null)
        {
            if (conversation != null)
            {
                await _followersRelationRepository.DeleteAsync(s => s.UserId == user.Id && s.ConversationId == conversation.Id);
            }

            if (contact != null)
            {
                await _followersRelationRepository.DeleteAsync(s => s.UserId == user.Id && s.ContactId == contact.Id);
            }

            await _unitOfWork.SaveChangesAsync();

        }
    }
}
