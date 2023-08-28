using AskPam.Crm.AI.Entities;
using AskPam.Crm.Configuration;
using AskPam.Exceptions;
using AskPam.Crm.Organizations;
using AskPam.Domain.Repositories;
using AskPam.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.AI
{
    public class QnAMakerManager : IQnAMakerManager
    {
        private readonly IQnAMakerService _qnAMakerService;
        private readonly IOrganizationManager _organizationManager;
        private readonly IRepository<QnAPair> _qnAPairRepository;
        private readonly ISettingManager _settingManager;
        private readonly IUnitOfWork _unitOfWork;

        public QnAMakerManager(
            IOrganizationManager organizationManager,
            ISettingManager settingManager,
            IQnAMakerService qnAMakerService,
            IRepository<QnAPair> qnAPairRepository, IUnitOfWork unitOfWork)
        {
            _qnAMakerService = qnAMakerService;
            _organizationManager = organizationManager;
            _settingManager = settingManager;
            _qnAPairRepository = qnAPairRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<QnAResult> Ask(string question, Guid organizationId)
        {
            var knowledgeBaseId = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotKnowledgeBaseId, organizationId);
            if (knowledgeBaseId == null)
            {
                throw new ApiException("Knowledgebase not registered.", HttpStatusCode.InternalServerError);
            }
            var answers = await _qnAMakerService.Ask(question, Guid.Parse(knowledgeBaseId));

            return answers
                .OrderByDescending(o => o.Score)
                .FirstOrDefault(); ;
        }

        public IQueryable<QnAPair> GetAllQnAPairs(Guid organizationId)
        {
            return _qnAPairRepository.GetAll()
                .Where(qnA => qnA.OrganizationId == organizationId);
        }

        public async Task<IEnumerable<QnAPair>> FindbyIds(IEnumerable<long> ids, Guid organizationId)
        {
            return await GetAllQnAPairs(organizationId)
                .Where(qnA => ids.Contains(qnA.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<QnAPair>> SaveKnowledgeBase(
            IEnumerable<QnAPair> newQnAs,
            IEnumerable<OldQnAPair> editQnAs,
            IEnumerable<QnAPair> delQnAs,
            Guid organizationId
        )
        {
            var knowledgeBaseId = await GetKnowledgeBaseId();
            if (!knowledgeBaseId.HasValue)
            {
                var organization = await _organizationManager.FindByIdAsync(organizationId);
                knowledgeBaseId = await _qnAMakerService.CreateKnowledgeBase(organization.Name, newQnAs);
                await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.AI.QnABotKnowledgeBaseId, knowledgeBaseId.Value.ToString(), organizationId);
            }
            else
            {
                await _qnAMakerService.UpdateKnowledgeBase(knowledgeBaseId.Value, newQnAs, editQnAs, delQnAs);
            }
            await _qnAMakerService.Publish(knowledgeBaseId.Value);

            var result = new List<QnAPair>();

            if (newQnAs != null)
            {
                foreach (var qnA in newQnAs)
                {
                    result.Add(await _qnAPairRepository.InsertAsync(qnA));
                }
            }

            if (editQnAs != null)
            {
                foreach (var oldQnA in editQnAs)
                {
                    var qna = oldQnA.Old;
                    qna.UpdateQuestion(oldQnA.NewQuestion);
                    qna.UpdateAnswer(oldQnA.NewAnswer);
                    result.Add(await _qnAPairRepository.UpdateAsync(qna));
                }
            }

            if (delQnAs != null)
            {
                foreach (var delQnA in delQnAs)
                {
                    await _qnAPairRepository.DeleteAsync(delQnA);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return result;

            async Task<Guid?> GetKnowledgeBaseId()
            {
                var setting = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotKnowledgeBaseId, organizationId);
                if (!string.IsNullOrEmpty(setting))
                {
                    return Guid.Parse(setting);
                }
                return null;
            }
        }
    }
}
