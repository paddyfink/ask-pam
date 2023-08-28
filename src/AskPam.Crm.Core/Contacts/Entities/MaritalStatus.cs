using System;
using System.Collections.Generic;
using System.Text;
using AskPam.Domain.Entities;

namespace AskPam.Crm.Contacts
{
    public class MaritalStatus : Enumeration<MaritalStatus, string>
    {
        public static readonly MaritalStatus None = new MaritalStatus("", "");
        public static readonly MaritalStatus Single = new MaritalStatus("Single", "Single");
        public static readonly MaritalStatus Married = new MaritalStatus("Married", "Married");
        public static readonly MaritalStatus Divorced = new MaritalStatus("Divorced", "Divorced");

        private MaritalStatus(string value, string displayName) : base(value, displayName) { }
    }
}
