using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AskPam.Crm.Common
{
    public class Phone
    {

        public string CountryCode { get; private set; }
        [MaxLength(20)]
        public string Number { get; private set; }
        public string NationalFormat { get; private set; }

        public Phone()
        {

        }

        public Phone(string number, string countryCode)
        {
            Number = number;
            CountryCode = countryCode;
        }

        public Phone(string number, string countryCode = null, string nationalFormat = null)
        {
            Number = number;
            if (countryCode != null) { CountryCode = countryCode; }
            if (nationalFormat != null) { NationalFormat = nationalFormat; }
        }

        public void Update(Phone mobilePhone)
        {
            Number = mobilePhone.Number;
            CountryCode = mobilePhone.CountryCode;
            NationalFormat = mobilePhone.NationalFormat;
        }
    }
}
