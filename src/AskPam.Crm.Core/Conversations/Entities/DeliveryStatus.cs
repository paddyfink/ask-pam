using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskPam.Crm.Conversations
{
    public class DeliveryStatus : Entity
    {
        public bool Success { get; set; }
        public bool Open { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? Date { get; set; }
        [NotMapped]
        public ChannelType ChannelType
        {
            get => Enumeration<ChannelType, string>.FromValue(ChannelTypeId);
            set => ChannelTypeId = value.Value;
        }
        [Column("Channel")]
        public string ChannelTypeId { get; set; }

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public long MessageId { get; set; }
        public virtual Message Message { get; set; }
    }

}
