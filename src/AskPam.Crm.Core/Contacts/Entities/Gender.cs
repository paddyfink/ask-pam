using System;
using System.Collections.Generic;
using System.Text;
using AskPam.Domain.Entities;

namespace AskPam.Crm.Contacts
{
    public class Gender : Enumeration<Gender, string>
    {
        public static readonly Gender None= new Gender("", "");
        public static readonly Gender Male= new Gender("Male", "Male");
        public static readonly Gender Female = new Gender("Female", "Female");

        private Gender(string value, string displayName) : base(value, displayName) { }
    }
}
