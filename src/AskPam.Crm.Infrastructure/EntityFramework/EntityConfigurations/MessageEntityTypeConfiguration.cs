using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskPam.Crm.EntityFramework.EntityConfigurations
{
    class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> messageConfiguration)
        {

            //messageConfiguration.OwnsOne(c => c.Status);
        }
    }
}
