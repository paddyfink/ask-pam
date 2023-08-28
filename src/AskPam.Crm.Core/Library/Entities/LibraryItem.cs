using AskPam.Crm.Organizations;
using AskPam.Crm.Common.Interfaces;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AskPam.Crm.Tags;

namespace AskPam.Crm.Library
{
    public class LibraryItem : Entity, IFullAudited, IMustHaveOrganization, ISoftDelete, IPassivable
    {
        [Required()]
        public string Name { get; set; }            //Mandatory
        public string Subject { get; set; }            
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneCountryCode { get; set; }
        public string NationalPhone { get; set; }
        public string Description { get; set; }
        public string OpeningHours { get; set; }
        public string Menu { get; set; }
        public string Area { get; set; }
        public string Price { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; private set; }
        public string Address2 { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string Province { get; private set; }
        public string Country { get; private set; }
        public LibraryItemType? Type { get; set; }

        //event
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAllDay { get; set; }

        //Settings
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedById { get; set; }
        public bool IsActive { get; set; }
  
        //Table + Foreign Keys
        public virtual ICollection<LibrarySupplierCategory> SupplierCategories { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<TagsRelation> TagsRelations { get; set; }

        internal LibraryItem()
        {

        }

        public LibraryItem(
            string name,
            Guid organizationId,
            string email = null,
            string phone = null,
            string description = null,
            string openinghours = null,
            string menu = null,
            string area = null,
            string price = null,
            string website = null,
            string fax = null,
            string address1 = null,
            string address2 = null,
            string postalCode = null,
            string city = null,
            string province = null,
            string country = null,
            LibraryItemType? type = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool? isAllDay = null,
            string subject = null
        )
        {
            Name = name;
            OrganizationId = organizationId;
            if (email != null) Email = email;
            if (phone != null) Phone = phone;
            if (description != null) Description = description;
            if (openinghours != null) OpeningHours = openinghours;
            if (menu != null) Menu = menu;
            if (area != null) Area = area;
            if (price != null) Price = price;
            if (website != null) Website = website;
            if (fax != null) Fax = fax;
            if (address1 != null) { Address1 = address1; }
            if (address2 != null) { Address2 = address2; }
            if (postalCode != null) { PostalCode = postalCode; }
            if (city != null) { City = city; }
            if (province != null) { Province = province; }
            if (country != null) { Country = country; }
            if (type.HasValue) { Type = type.Value; }
            if (startDate.HasValue) { StartDate = startDate.Value; }
            if (endDate.HasValue) { EndDate = endDate.Value; }
            if (isAllDay.HasValue) { IsAllDay = isAllDay.Value; }
            if (subject != null) { Subject = subject; }
        }
        public void Update(
            string name,
            string email = null,
            string phone = null,
            string description = null,
            string openinghours = null,
            string menu = null,
            string area = null,
            string price = null,
            string website = null,
            string fax = null,
            string address1 = null,
            string address2 = null,
            string postalCode = null,
            string city = null,
            string province = null,
            string country = null,
            LibraryItemType? type = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool? isAllDay = null,
            string subject = null
        )
        {
            Name = name;
            if (email != null) Email = email;
            if (phone != null) Phone = phone;
            if (description != null) Description = description;
            if (openinghours != null) OpeningHours = openinghours;
            if (menu != null) Menu = menu;
            if (area != null) Area = area;
            if (price != null) Price = price;
            if (website != null) Website = website;
            if (fax != null) Fax = fax;
            if (address1 != null) { Address1 = address1; }
            if (address2 != null) { Address2 = address2; }
            if (postalCode != null) { PostalCode = postalCode; }
            if (city != null) { City = city; }
            if (province != null) { Province = province; }
            if (country != null) { Country = country; }
            if (type.HasValue) { Type = type.Value; }
            if (startDate.HasValue) { StartDate = startDate.Value; }
            if (endDate.HasValue) { EndDate = endDate.Value; }
            if (isAllDay.HasValue) { IsAllDay = isAllDay.Value; }
            if (subject != null) { Subject = subject; }

        }

        public void SetPhoneCountryCode(string phoneCountryCode)
        {
            PhoneCountryCode = phoneCountryCode;
        }

        public void SetNationalPhone(string nationalPhone)
        {
            NationalPhone = nationalPhone;
        }
    }
}
