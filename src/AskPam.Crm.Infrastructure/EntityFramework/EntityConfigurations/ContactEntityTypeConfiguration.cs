using AskPam.Crm.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskPam.Crm.EntityFramework.EntityConfigurations
{
    class ContactEntityTypeConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> contactConfiguration)
        {
            contactConfiguration.Property(p => p.FullName)
           .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

            contactConfiguration.OwnsOne(c => c.Address);

            contactConfiguration.OwnsOne(c => c.MobilePhone);

            contactConfiguration.OwnsOne(c => c.BusinessPhone);
        }
    }
}
