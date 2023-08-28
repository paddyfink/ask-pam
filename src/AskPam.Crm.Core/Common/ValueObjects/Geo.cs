using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Common
{
    public class Geo
    {
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string RegionCode { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public float? Latitude { get; set; }
        public float? Lontitude { get; set; }
        public string Ip { get; set; }
    }
}
