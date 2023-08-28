using AskPam.Crm.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskPam.Crm.EntityFramework.EntityConfigurations
{
    class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> contactConfiguration)
        {
            contactConfiguration.HasKey(u => u.Id);
            contactConfiguration.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

            contactConfiguration
            .Property(p => p.FullName)
           .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

        }
    }
}
