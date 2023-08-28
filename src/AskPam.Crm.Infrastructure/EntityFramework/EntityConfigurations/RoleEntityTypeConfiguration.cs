using AskPam.Crm.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskPam.Crm.EntityFramework.EntityConfigurations
{
    class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> roleConfiguration)
        {
            roleConfiguration.HasKey(r => r.Id);
            roleConfiguration.HasMany(r => r.Users).WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
        }
    }
}
