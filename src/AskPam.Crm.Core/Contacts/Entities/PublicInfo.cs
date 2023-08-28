using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Contacts
{
    public class PublicInfo : Entity
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Data { get; set; }
    }
}
