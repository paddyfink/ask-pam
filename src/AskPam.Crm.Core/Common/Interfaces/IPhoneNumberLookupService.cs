using System.Threading.Tasks;

namespace AskPam.Crm.Common
{
    public interface IPhoneNumberLookupService
    {
        Task<Phone> Format(string phoneNumber, bool forceValidation=true);
    }
}
