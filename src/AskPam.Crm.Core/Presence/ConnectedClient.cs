using AskPam.Crm.Authorization;
using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskPam.Crm.Presence
{
    [Table("ConnectedClients")]
    public class ConnectedClient : Entity<long>
    {
        [StringLength(100)]
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public DateTime LastActivity { get; set; }

        //Relationship
        public User User { get; set; }

    }
}
