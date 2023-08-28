using AskPam.Crm.Authorization;
using AskPam.Crm.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskPam.Crm.EntityFramework.EntityConfigurations
{
    class TagsEntityTypeConfiguration : IEntityTypeConfiguration<TagsRelation>
    {
        public void Configure(EntityTypeBuilder<TagsRelation> tagConfiguration)
        {
            tagConfiguration
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.TagsRelations)
            .HasForeignKey(pt => pt.TagId);

            tagConfiguration
            .HasOne(pt => pt.Contact)
            .WithMany(p => p.TagsRelations)
            .HasForeignKey(pt => pt.ContactId);

            tagConfiguration
            .HasOne(pt => pt.Library)
            .WithMany(p => p.TagsRelations)
            .HasForeignKey(pt => pt.LibraryItemId);

            tagConfiguration
            .HasOne(pt => pt.Conversation)
            .WithMany(p => p.TagsRelations)
            .HasForeignKey(pt => pt.ConversationId);
        }
    }
}
