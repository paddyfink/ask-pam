using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Contacts.Dtos
{
    public class SimpleContactDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhoneNumber { get; set; }
    }
}
