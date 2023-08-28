using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AskPam.Crm.Authorization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using Z.EntityFramework.Plus;
using AskPam.Crm.Common.Interfaces;
using AskPam.Domain.Entities.Auditing;
using AskPam.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using AskPam.Crm.Organizations;
using AskPam.Crm.Runtime.Session;
using AskPam.Crm.Library;
using AskPam.Crm.Notifications;
using AskPam.Crm.Authorization.Followers;
using AskPam.Crm.Tags;
using AskPam.Crm.Configuration;
using AskPam.Crm.AI.Entities;
using AskPam.Crm.Presence;
using AskPam.Crm.Posts;
using AskPam.Crm.Stars;
using AskPam.Crm.InternalNotes;
using AskPam.Crm.EntityFramework.EntityConfigurations;

namespace AskPam.Crm.EntityFramework
{
    public class CrmDbContext : DbContext
    {
        private ICrmSession Session { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<FollowersRelation> FollowersRelations { get; set; }
        //public DbSet<StarsRelation> StarsRelation { get; set; }
        public DbSet<Tag> Tags { get; set; }
        //public DbSet<TagsRelation> TagsRelations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactGroup> ContactGroups { get; set; }
        public DbSet<LibraryItem> LibraryItems { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        //public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Setting> Settings { get; set; }
        //public DbSet<Channel> Channels { get; set; }
        //public DbSet<DeliveryStatus> DeliveryStatuses { get; set; }
        public DbSet<QnAPair> QnAPairs { get; set; }
        public DbSet<ConnectedClient> ConnectedClients { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<InternalNote> InternalNotes { get; set; }



        public CrmDbContext(DbContextOptions options, ICrmSession session)
            : base(options)
        {
            Session = session;
        }

        public void SetGlobalQuery(ModelBuilder builder)
        {
            builder.Entity<Contact>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<ContactGroup>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<LibraryItem>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Conversation>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Post>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Message>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Organization>().HasQueryFilter(e => !e.IsDeleted);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ConversationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TagsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEntityTypeConfiguration());
            

            SetGlobalQuery(modelBuilder);

            //builder.Entity<Notification>(b =>
            //{
            //    b.HasKey(u => u.Id);
            //    b.HasMany(u => u.Users).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            //});
            
            modelBuilder.Entity<Email>();
            modelBuilder.Entity<Message>();


            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }


        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified));


            var entries = ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        CheckAndSetMustHaveOrganizationIddProperty(entry.Entity);
                        SetCreationAuditProperties(entry.Entity);
                        break;
                    case EntityState.Modified:
                        SetModificationAuditProperties(entry);
                        if (entry.Entity is ISoftDelete && ((ISoftDelete)entry.Entity).IsDeleted)
                        {
                            SetDeletionAuditProperties(entry.Entity);
                        }
                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
                        SetDeletionAuditProperties(entry.Entity);
                        break;
                }
            }
        }


        private void CheckAndSetMustHaveOrganizationIddProperty(object entityAsObj)
        {
            //Only set IMustHaveTenant entities
            if (!(entityAsObj is IMustHaveOrganization))
            {
                return;
            }

            var entity = (IMustHaveOrganization)entityAsObj;

            //Don't set if it's already set
            if (entity.OrganizationId != Guid.Empty)
            {
                return;
            }

            var currentTenantId = Session.OrganizationId;

            if (Session.OrganizationId.HasValue)
            {
                entity.OrganizationId = Session.OrganizationId.Value;
            }
            else
            {
                throw new Exception("Can not set CurrentOrganization to 0 for IMustHaveOrganization entities!");
            }
        }

        private void SetCreationAuditProperties(object entityAsObj)
        {
            if (!(entityAsObj is ICreationAudited entityWithCreatedAt))
            {
                return;
            }

            if (entityWithCreatedAt.CreatedAt == null)
            {
                entityWithCreatedAt.CreatedAt = DateTime.UtcNow;
            }

            if (!string.IsNullOrEmpty(Session.UserId) && entityAsObj is ICreationAudited)
            {
                var entity = entityAsObj as ICreationAudited;
                if (string.IsNullOrEmpty(entity.CreatedById))
                {
                    if (entity is IMustHaveOrganization)
                    {
                        //Sets CreatorUserId only if current user is in same tenant/host with the given entity
                        if (entity is IMustHaveOrganization && ((IMustHaveOrganization)entity).OrganizationId == Session.OrganizationId)
                        {
                            entity.CreatedById = Session.UserId;
                        }
                    }
                    else
                    {
                        entity.CreatedById = Session.UserId;
                    }
                }
            }
        }


        private void SetModificationAuditProperties(EntityEntry entry)
        {
            if (entry.Entity is IModificationAudited)
            {
                ((IModificationAudited)entry.Entity).ModifiedAt = DateTime.UtcNow;
            }

            if (entry.Entity is IModificationAudited entity)
            {
                if (string.IsNullOrEmpty(Session.UserId))
                {
                    entity.ModifiedById = string.Empty;
                    return;
                }

                //Special check for multi-tenant entities
                if (entity is IMustHaveOrganization)
                {
                    //Sets LastModifierUserId only if current user is in same tenant/host with the given entity
                    if (((IMustHaveOrganization)entry.Entity).OrganizationId == Session.OrganizationId)
                    {
                        entity.ModifiedById = Session.UserId;
                    }
                    else
                    {
                        entity.ModifiedById = string.Empty;
                    }
                }
                else
                {
                    entity.ModifiedById = Session.UserId;
                }
            }
        }

        private void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            entry.State = EntityState.Unchanged; //TODO: Or Modified? IsDeleted = true makes it modified?
            ((ISoftDelete)entry.Entity).IsDeleted = true;
        }

        private void SetDeletionAuditProperties(object entityAsObj)
        {
            if (entityAsObj is IDeletionAudited)
            {
                var entity = (IDeletionAudited)entityAsObj;

                if (entity.DeletedAt == null)
                {
                    entity.DeletedAt = DateTime.Now;
                }
            }

            if (entityAsObj is IDeletionAudited)
            {
                var entity = (IDeletionAudited)entityAsObj;

                if (string.IsNullOrEmpty(entity.DeletedById))
                {
                    return;
                }

                if (string.IsNullOrEmpty(Session.UserId))
                {
                    entity.DeletedById = string.Empty;
                    return;
                }

                //Special check for multi-tenant entities
                if (entity is IMustHaveOrganization)
                {
                    //Sets LastModifierUserId only if current user is in same tenant/host with the given entity
                    if ((entity is IMustHaveOrganization && ((IMustHaveOrganization)entity).OrganizationId == Session.OrganizationId))
                    {
                        entity.DeletedById = Session.UserId;
                    }
                    else
                    {
                        entity.DeletedById = string.Empty;
                    }
                }
                else
                {
                    entity.DeletedById = Session.UserId;
                }
            }
        }

    }
}
