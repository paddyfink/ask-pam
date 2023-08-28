using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Common
{
    public class Address
    {
        public string Address1 { get; private set; }
        public string Address2 { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string Province { get; private set; }
        public string Country { get; private set; }

        public Address()
        {

        }

        public Address(string address1, string address2, string postalCode,string city, string provine, string country)
        {
            Address1 = address1;
            Address2 = address2;
            PostalCode = postalCode;
            City = city;
            Province = provine;
            Country = country;
        }

        public void Udpate(Address address)
        {
            Address1 = address.Address1;
            Address2 = address.Address2;
            PostalCode = address.PostalCode;
            City = address.City;
            Province = address.Province;
            Country = address.Country;
        }
    }
}
