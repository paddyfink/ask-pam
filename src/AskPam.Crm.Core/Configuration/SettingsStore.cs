using AskPam.Domain.Repositories;
using AskPam.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.Configuration
{
    public class SettingStore : ISettingStore
    {
        private readonly IRepository<Setting, long> _settingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SettingStore(IRepository<Setting, long> settingRepository, IUnitOfWork unitOfWork)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Setting>> GetAllListAsync(Guid? organizationId, string userId)
        {
            var query = _settingRepository.GetAll()
                .Where(s => s.OrganizationId == organizationId);

            if (!userId.IsNullOrEmpty())
                query = query.Where(s => s.UserId == userId);

            return await query.ToListAsync();
        }

        public async Task<Setting> GetSettingOrNullAsync(Guid? organizationId, string userId, string name)
        {
            var query = _settingRepository.GetAll()
               .Where(s => s.OrganizationId == organizationId && s.Name == name);

            if (!userId.IsNullOrEmpty())
                query = query.Where(s => s.UserId == userId);

            return await query.FirstOrDefaultAsync();

        }


        public async Task DeleteAsync(Setting setting)
        {
            await _settingRepository.DeleteAsync(s => s.UserId == setting.UserId && s.Name == setting.Name && s.OrganizationId == setting.OrganizationId);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task CreateAsync(Setting setting)
        {

            await _settingRepository.InsertAsync(setting);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task UpdateAsync(Setting settingInfo)
        {

            var setting = await _settingRepository.FirstOrDefaultAsync(
                s => s.OrganizationId == settingInfo.OrganizationId &&
                     s.UserId == settingInfo.UserId &&
                     s.Name == settingInfo.Name
                );

            if (setting != null)
            {
                setting.Value = settingInfo.Value;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
