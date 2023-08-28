using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskPam.Extensions;

namespace AskPam.Crm.Common.Dtos
{
    public class AddressDto
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

        public string FullAddresse
        {
            get
            {
                var ad = new StringBuilder();
                if (!Address1.IsNullOrEmpty()) ad.Append(Address1);
                if (!Address2.IsNullOrEmpty()) ad.AppendLine(Address2);
                if ((!City.IsNullOrEmpty())) ad.AppendLine(City);
                if (!PostalCode.IsNullOrEmpty()) ad.AppendJoin(" ", PostalCode);
                if ((!Province.IsNullOrEmpty())) ad.AppendLine(Province);
                if (!Country.IsNullOrEmpty()) ad.AppendJoin(" ", Country);

                return ad.ToString();
            }
        }
    }
}
