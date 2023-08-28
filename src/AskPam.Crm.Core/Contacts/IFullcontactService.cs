using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Crm.Contacts
{
    public interface IFullContactService
    {
        Task<string> Search(string email = null, string phone = null);
    }
}
