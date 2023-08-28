using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations.Dtos.PostMark
{
    public class Client
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Family { get; set; }
    }

    public class OS
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Family { get; set; }
    }

    public class Geo
    {
        public string CountryISOCode { get; set; }
        public string Country { get; set; }
        public string RegionISOCode { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Coords { get; set; }
        public string IP { get; set; }
    }
    public class OpenedEmail
    {
        public bool FirstOpen { get; set; }
        public Client Client { get; set; }
        public OS OS { get; set; }
        public string Platform { get; set; }
        public string UserAgent { get; set; }
        public int ReadSeconds { get; set; }
        public Geo Geo { get; set; }
        public string MessageID { get; set; }
        public string ReceivedAt { get; set; }
        public string Tag { get; set; }
        public string Recipient { get; set; }
    }
}
