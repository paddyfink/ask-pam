using System;
using System.Collections.Generic;
using System.Text;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskPam.Crm.EntityFramework.EntityConfigurations
{
    public class ConversationEntityTypeConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> conversationConfiguration)
        {

            conversationConfiguration.OwnsOne(c => c.LastLocation);
        }
    }
}
