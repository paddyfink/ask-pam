using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Common.Dtos
{
    public class PhoneDto
    {
        public string CountryCode { get; set; }
        public string Number { get; set; }
        public string NationalFormat { get; set; }

        //public string InternationalFormat
        //{
        //    get
        //    {

        //        PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();


        //        try
        //        {
        //            var phoneNumber = phoneUtil.Parse(Number, CountryCode);
        //            return phoneUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
        //        }
        //        catch (Exception)
        //        {
        //            return string.IsNullOrEmpty(NationalFormat) ? Number : NationalFormat;
        //        }
        //    }
        //}
    }
}
