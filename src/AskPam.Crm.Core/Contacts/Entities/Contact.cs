using AskPam.Crm.Organizations;
using AskPam.Crm.Common.Interfaces;
using AskPam.Crm.Conversations;
using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AskPam.Crm.Authorization;
using AskPam.Crm.Tags;
using AskPam.Crm.InternalNotes;
using AskPam.Crm.Stars;
using AskPam.Crm.Common;

namespace AskPam.Crm.Contacts
{
    public class Contact : FullAuditedEntity, IMustHaveOrganization, ISoftDelete, IAggregateRoot
    {

        [Required]
        public string FirstName { get; private set; }
        [Required]
        public string LastName { get; private set; }
        public string FullName { get; private set; }
        public string EmailAddress { get; private set; }
        public string EmailAddress2 { get; private set; }
        public DateTime? DateOfBirth { get; private set; }

        [NotMapped]
        public Gender Gender
        {
            get => Enumeration<Gender, string>.FromValue(GenderEnumValue);
            set => GenderEnumValue = value.Value;
        }
        [Column("Gender")]
        public string GenderEnumValue { get; set; }

        [NotMapped]
        public MaritalStatus MaritalStatus
        {
            get => Enumeration<MaritalStatus, string>.FromValue(MaritalStatusEnumValue);
            set => MaritalStatusEnumValue = value.Value;
        }

        [Column("MaritalStatus")]
        public string MaritalStatusEnumValue { get; set; }
        public string PrimaryLanguage { get; private set; }
        public string SecondaryLanguage { get; private set; }
        public Phone MobilePhone { get; private set; }
        public Address Address { get; private set; }
        public string Bio { get; private set; }

        public string JobTitle { get; private set; }
        public string Company { get; private set; }
        public Phone BusinessPhone { get; private set; }
        public string AssignedToId { get; private set; }
        public string CustomFields { get; private set; }
        public virtual User AssignedTo { get; set; }
        public DateTime? AssignedToDate { get; set; }

        public string ExternalId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid Uid { get; set; }

        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public long? GroupId { get; set; }
        public virtual ContactGroup Group { get; set; }
        public virtual ICollection<InternalNote> InternalNotes { get; set; }
        public virtual ICollection<TagsRelation> TagsRelations { get; set; }
        public virtual ICollection<StarsRelation> StarRelations { get; set; }
        public virtual ICollection<Conversation> Conversations { get; set; }

        public string SmoochUserId { get; set; }

        //public int? PublicInfoId { get; set; }
        //public virtual PublicInfo PublicInfo { get; set; }

        internal Contact()
        {
        }

        public Contact(

            string firstName,
            string lastName,
            Guid organizationId,
            string emailAddress = null,
            Gender gender = null,
            DateTime? dateOfBirthDay = null,
            MaritalStatus maritalStatus = null,
            string primaryLanguage = null,
            string secondaryLanguage = null,
            Address address = null,
            string bio = null,

            //Company
            string company = null,
            string jobTitle = null,

            //Group
            long? groupId = null,

            string externalId = null,
            string smoochUserId = null,
            string customFields = null,
            string emailAddress2 = null
        )
        {
            FirstName = firstName;
            LastName = lastName;
            OrganizationId = organizationId;
            Uid = new Guid();
            MobilePhone = new Phone();
            Address = new Address();
            Gender = gender ?? Gender.None;
            MaritalStatus = maritalStatus ?? MaritalStatus.None;
            if (dateOfBirthDay.HasValue) { DateOfBirth = dateOfBirthDay.Value; }
            if (primaryLanguage != null) { PrimaryLanguage = primaryLanguage; }
            if (emailAddress != null) { EmailAddress = emailAddress; }
            if (emailAddress2 != null) { EmailAddress2 = emailAddress2; }
            if (secondaryLanguage != null) { SecondaryLanguage = secondaryLanguage; }
            if (bio != null) { Bio = bio; }
            if (company != null) { Company = company; }
            if (jobTitle != null) { JobTitle = jobTitle; }
            if (groupId.HasValue) { GroupId = groupId.Value; }
            if (externalId != null) ExternalId = externalId;
            if (address != null) Address = address;
            if (smoochUserId != null) SmoochUserId = smoochUserId;
            if (address != null)
                Address.Udpate(address);
            if (customFields != null) CustomFields = customFields;
        }


        public void Update(
            //Personal
            string firstName,
            string lastName,
            Guid organizationId,
            string emailAddress = null,
            string emailAddress2 = null,
            Gender gender = null,
            DateTime? dateOfBirth = null,
            MaritalStatus maritalStatus = null,
            string primaryLanguage = null,
            string secondaryLanguage = null,
            Address address = null,
            string bio = null,

            //Company
            string company = null,
            string jobTitle = null,

            //Group
            long? groupId = null,
            string externalId = null,
            string customFields = null

            )
        {
            FirstName = firstName;
            LastName = lastName;
            OrganizationId = organizationId;
            if (emailAddress != null) { EmailAddress = emailAddress; }
            if (emailAddress2 != null) { EmailAddress2 = emailAddress2; }
            if (gender != null) { Gender = gender; }
            if (dateOfBirth.HasValue) { DateOfBirth = dateOfBirth.Value; }
            if (maritalStatus != null) { MaritalStatus = maritalStatus; }
            if (primaryLanguage != null) { PrimaryLanguage = primaryLanguage; }
            if (secondaryLanguage != null) { SecondaryLanguage = secondaryLanguage; }
            if (bio != null) { Bio = bio; }
            if (company != null) { Company = company; }
            if (jobTitle != null) { JobTitle = jobTitle; }
            if (groupId.HasValue) { GroupId = groupId.Value; }
            if (externalId != null) ExternalId = externalId;
            if (customFields != null) CustomFields = customFields;
            Address.Udpate(address);
        }

        public void UpdateField(string field, object value)
        {
            if (field == nameof(FirstName) && value is string)
                FirstName = (string)value;

            if (field == nameof(LastName) && value is string)
                LastName = (string)value;

            if (field == nameof(EmailAddress) && value is string)
                EmailAddress = ((string)value).ToLower();

            if (field == nameof(EmailAddress2) && value is string)
                EmailAddress2 = ((string)value).ToLower();

            if (field == nameof(DateOfBirth) && value is DateTime)
                DateOfBirth = (DateTime)value;

            if (field == nameof(PrimaryLanguage) && value is string)
                PrimaryLanguage = (string)value;

            if (field == nameof(SecondaryLanguage) && value is string)
                SecondaryLanguage = (string)value;


            if (field == nameof(Bio) && value is string)
                Bio = (string)value;

            if (field == nameof(Company) && value is string)
                Company = (string)value;

            if (field == nameof(JobTitle) && value is string)
                JobTitle = (string)value;

            if (field == nameof(ExternalId) && value is string)
                ExternalId = (string)value;

            if (field == nameof(CustomFields) && value is string)
                CustomFields = (string)value;

            if (field == nameof(GroupId))
                GroupId = (int?)value;

        }

        public void AssignToUser(string userId)
        {
            AssignedToId = userId;
            AssignedToDate = DateTime.UtcNow;
        }

        public void UnAssign()
        {
            AssignedToId = null;
        }

        public void AddToGroup(ContactGroup group)
        {
            GroupId = group.Id;
        }


        public void UpdateMobilePhone(Phone mobilePhone)
        {
            MobilePhone.Update(mobilePhone);
        }
    }
}
