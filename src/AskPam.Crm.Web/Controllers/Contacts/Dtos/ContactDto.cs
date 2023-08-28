using AskPam.Application.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using AskPam.Crm.Tags.Dtos;
using System.Collections.Generic;
using AskPam.Crm.Common.Dtos;
using AskPam.Crm.Conversations.Dtos;

namespace AskPam.Crm.Contacts.Dtos
{
    public class ContactDto : FullAuditedDto
    {
        public int Id { get; set; }

        //Personal Information
        //Personal Information
        public string GenderDisplayName { get; set; } 
        public string Gender { get; set; }
        [Required]
        public string FirstName { get; set; } 
        [Required]
        public string LastName { get; set; } 
        public string FullName => $"{FirstName} {LastName}";
        public DateTime? DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusDisplayName { get; set; }
        public string PrimaryLanguage { get; set; }
        public string SecondaryLanguage { get; set; }
        
        public string Bio { get; set; }
        public string AssignedToFullName { get; set; }
        public dynamic PublicInfo { get; set; }
        public dynamic Data { get; set; }
        public string AssignedToId { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public bool IsNew { get; set; }
        public string EmailAddress { get; set; }
        public string EmailAddress2 { get; set; }
        public AddressDto Address { get; set; }
        public string MobilePhone { get; set; }
        public string MobilePhoneDisplay { get; set; }
        //Company
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public dynamic CustomFields { get; set; }
        //Foreign keys
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        
      
    }
}
