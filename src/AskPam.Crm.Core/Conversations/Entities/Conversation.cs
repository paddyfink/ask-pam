using AskPam.Crm.Common.Interfaces;
using AskPam.Crm.Contacts;
using AskPam.Domain.Entities;
using System.Collections.Generic;
using AskPam.Crm.Organizations;
using System;
using AskPam.Domain.Entities.Auditing;
using AskPam.Crm.Authorization;
using System.ComponentModel.DataAnnotations;
using AskPam.Crm.Authorization.Followers;
using AskPam.Crm.Common;
using AskPam.Crm.Tags;
using AskPam.Crm.Stars;

namespace AskPam.Crm.Conversations
{
    public class Conversation : FullAuditedEntity, IMustHaveOrganization, ISoftDelete, IAggregateRoot
    {
        [Required]

        public bool Seen { get; set; }
        public bool IsFlagged { get; private set; }
        public string AvatarColor { get; set; }
        public bool IsActive { get; private set; }
        public bool BotDisabled { get; private set; }


        public bool IsDeleted { get; set; }

        public string Name { get; private set; }
        public string SmoochUserId { get; set; }
        public string Email { get; set; }

        public Geo LastLocation { get;  set; }
        public long? ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        //Tables + Foreigns Keys
        public virtual ICollection<Message> Messages { get; private set; }
        public virtual ICollection<Channel> Channels { get; private set; }

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public string AssignedToId { get; private set; }
        public virtual User AssignedTo { get; set; }
        public virtual ICollection<FollowersRelation> Followers { get; set; }
        public virtual ICollection<StarsRelation> StarRelations { get; set; }
        public virtual ICollection<TagsRelation> TagsRelations { get; set; }

        internal Conversation()
        {

        }
        public Conversation(
            Guid organizationId,
            string name,
            string smoochUserId = null,
            string email = null,
            string color = null,
            long? contactId = null,
            bool? seen = null,
            string assignedTo = null,
            bool botDisabled = false,
            bool isArchived = false,
            bool isFlagged = false
        )
        {
            IsActive = true;
            Uid = Guid.NewGuid();
            Name = name;
            OrganizationId = organizationId;
            SmoochUserId = smoochUserId;
            Email = email;
            ContactId = contactId;
            if (seen.HasValue) { Seen = seen.Value; }

            if (!string.IsNullOrEmpty(assignedTo))
                AssignedToId = assignedTo;

            if (!string.IsNullOrEmpty(color))
                AvatarColor = color;

            BotDisabled = botDisabled;
            IsActive = !isArchived;
            IsFlagged = isFlagged;
            Channels = new List<Channel>();
            LastLocation = new Geo();
        }


        public void AddMessage(Message message)
        {
            message.ConversationId = Id;
            if (Messages == null)
                Messages = new List<Message>();

            Messages.Add(message);
        }

        public void AddChannel(Channel channel)
        {
            channel.ConversationId = Id;
            if (Channels == null)
                Channels = new List<Channel>();

            Channels.Add(channel);
        }
        public void AssignToUser(string userId)
        {
            AssignedToId = userId;
        }
        public void RemoveAssignment()
        {
            AssignedToId = null;
        }

        public void Flag()
        {
            IsFlagged = !IsFlagged;
        }



        public void Active()
        {
            IsActive = !IsActive;
        }
        public void ToogleBotActivation()
        {
            BotDisabled = !BotDisabled;
        }


        public void MarkAsSeen()
        {
            Seen = true;
        }
        public void MarkAsUnSeen()
        {
            Seen = false;
        }

        public void AttachToContact(long? contactId)
        {
            ContactId = contactId;
        }

        public void UpdateLastLocation(Geo location)
        {
            if (location==null)
                throw new ArgumentException(nameof(location));
            if (LastLocation == null)
            {
                LastLocation = new Geo();
            }

            LastLocation.Country = location.Country;
            LastLocation.City = location.City;
            LastLocation.CountryCode = location.CountryCode;
            LastLocation.Ip = location.Ip;
            LastLocation.Latitude = location.Latitude;
            LastLocation.Lontitude = location.Lontitude;
            LastLocation.Region = location.Region;
            LastLocation.RegionCode = location.RegionCode;
            LastLocation.Zip = location.Zip;

        }

        public Guid Uid { get; set; }
    }
}
