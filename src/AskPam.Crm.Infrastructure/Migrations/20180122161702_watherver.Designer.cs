﻿// <auto-generated />
using AskPam.Crm.EntityFramework;
using AskPam.Crm.Library;
using AskPam.Crm.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace AskPam.Crm.Migrations
{
    [DbContext(typeof(CrmDbContext))]
    [Migration("20180122161702_watherver")]
    partial class watherver
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AskPam.Crm.AI.Entities.QnAPair", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("OrganizationId");

                    b.Property<string>("Question");

                    b.HasKey("Id");

                    b.ToTable("QnAPairs");
                });

            modelBuilder.Entity("AskPam.Crm.Authorization.Followers.FollowersRelation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ConversationId");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<long?>("PostId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("FollowersRelation");
                });

            modelBuilder.Entity("AskPam.Crm.Authorization.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<bool>("IsDefault");

                    b.Property<bool>("IsStatic");

                    b.Property<string>("Name");

                    b.Property<Guid?>("OrganizationId");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("AskPam.Crm.Authorization.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("Email");

                    b.Property<bool?>("EmailVerified");

                    b.Property<string>("ExternalId");

                    b.Property<string>("FirstName");

                    b.Property<string>("FullName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Picture");

                    b.Property<string>("Signature");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AskPam.Crm.Authorization.UserOrganization", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Default");

                    b.Property<Guid>("OrganizationId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserOrganizations");
                });

            modelBuilder.Entity("AskPam.Crm.Authorization.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("OrganizationId");

                    b.Property<long>("RoleId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("AskPam.Crm.Configuration.Setting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<Guid?>("OrganizationId");

                    b.Property<string>("UserId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("UserId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("AskPam.Crm.Contacts.Contact", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bio");

                    b.Property<string>("Company");

                    b.Property<bool>("ConversationStarted");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("ExternalId");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

                    b.Property<string>("GenderId");

                    b.Property<long?>("GroupId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("JobTitle");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("MaritalStatusId");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<Guid>("OrganizationId");

                    b.Property<string>("PrimaryLanguage");

                    b.Property<string>("SecondaryLanguage");

                    b.Property<string>("SmoochUserId");

                    b.Property<Guid>("Uid");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("AskPam.Crm.Contacts.ContactGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid>("OrganizationId");

                    b.HasKey("Id");

                    b.ToTable("ContactGroups");
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.Attachment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Content");

                    b.Property<int>("ContentLength");

                    b.Property<string>("ContentType");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<long>("MessageId");

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.Channel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("AvatarUrl");

                    b.Property<long?>("ContactId");

                    b.Property<long?>("ConversationId");

                    b.Property<string>("DisplayName");

                    b.Property<DateTime?>("LastSeen");

                    b.Property<Guid>("OrganizationId");

                    b.Property<bool>("Primary");

                    b.Property<string>("SmoochId");

                    b.Property<string>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("ConversationId");

                    b.ToTable("Channel");
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.Conversation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AssignedToId");

                    b.Property<string>("AvatarColor");

                    b.Property<bool>("BotDisabled");

                    b.Property<long?>("ContactId");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("Email");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsFlagged");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name");

                    b.Property<Guid>("OrganizationId");

                    b.Property<bool>("Seen");

                    b.Property<string>("SmoochUserId");

                    b.Property<Guid>("Uid");

                    b.HasKey("Id");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("ContactId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.DeliveryStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChannelTypeId");

                    b.Property<DateTime?>("Date");

                    b.Property<string>("ErrorCode");

                    b.Property<string>("ErrorMessage");

                    b.Property<long>("MessageId");

                    b.Property<bool>("Open");

                    b.Property<DateTime?>("OpenDate");

                    b.Property<bool>("Success");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("DeliveryStatus");
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("AuthorId");

                    b.Property<string>("Avatar");

                    b.Property<string>("ChannelTypeId");

                    b.Property<long>("ConversationId");

                    b.Property<string>("CreatedUserId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<bool>("IsAutomaticReply");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("MessageStatusId");

                    b.Property<string>("MessageTypeId");

                    b.Property<Guid?>("PostmarkId");

                    b.Property<long?>("ReplyTo");

                    b.Property<bool?>("Seen");

                    b.Property<string>("SmoochMessageId");

                    b.Property<string>("Subject");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("Message");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Message");
                });

            modelBuilder.Entity("AskPam.Crm.InternalNotes.InternalNote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int?>("ContactId");

                    b.Property<long?>("ContactId1");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<string>("CreatedUserId");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("LibraryId");

                    b.Property<int?>("LibraryItemId");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<Guid>("OrganizationId");

                    b.Property<int?>("PostId");

                    b.Property<long?>("PostId1");

                    b.HasKey("Id");

                    b.HasIndex("ContactId1");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("LibraryId");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("PostId1");

                    b.ToTable("InternalNotes");
                });

            modelBuilder.Entity("AskPam.Crm.Library.LibraryItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("Area");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("Fax");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsAllDay");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Menu");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NationalPhone");

                    b.Property<string>("OpeningHours");

                    b.Property<Guid>("OrganizationId");

                    b.Property<string>("Phone");

                    b.Property<string>("PhoneCountryCode");

                    b.Property<string>("PostalCode");

                    b.Property<string>("Price");

                    b.Property<string>("Province");

                    b.Property<DateTime?>("StartDate");

                    b.Property<string>("Subject");

                    b.Property<int?>("Type");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("LibraryItems");
                });

            modelBuilder.Entity("AskPam.Crm.Notifications.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<string>("CreatedUserId");

                    b.Property<string>("Data");

                    b.Property<string>("EntityId");

                    b.Property<string>("EntityType");

                    b.Property<string>("NotificationType");

                    b.Property<Guid>("OrganizationId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("AskPam.Crm.Notifications.UserNotification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("NotificationId");

                    b.Property<Guid>("OrganizationId");

                    b.Property<bool>("Read");

                    b.Property<bool>("Seen");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId");

                    b.ToTable("UserNotifications");
                });

            modelBuilder.Entity("AskPam.Crm.Organizations.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("BrainDates");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("Email");

                    b.Property<string>("EmailName");

                    b.Property<bool>("FullContact");

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsDkimVerified");

                    b.Property<bool>("IsSpfVerified");

                    b.Property<bool>("Klik");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name");

                    b.Property<string>("PostmarkSenderId");

                    b.Property<string>("SmoochAppId");

                    b.Property<string>("SmoochAppToken");

                    b.Property<bool>("Stay22");

                    b.Property<int?>("Type");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("AskPam.Crm.Posts.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<string>("CreatedUserId");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<Guid>("OrganizationId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("AskPam.Crm.Presence.ConnectedClient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConnectionId")
                        .HasMaxLength(100);

                    b.Property<DateTime>("LastActivity");

                    b.Property<string>("UserAgent");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ConnectedClients");
                });

            modelBuilder.Entity("AskPam.Crm.Stars.StarsRelation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ContactId");

                    b.Property<long?>("ConversationId");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("ConversationId");

                    b.ToTable("StarsRelation");
                });

            modelBuilder.Entity("AskPam.Crm.Tags.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<Guid>("OrganizationId");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("AskPam.Crm.Tags.TagsRelation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ContactId");

                    b.Property<long?>("ConversationId");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<long?>("LibraryItemId");

                    b.Property<long?>("PostId");

                    b.Property<long>("TagId");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("ConversationId");

                    b.HasIndex("LibraryItemId");

                    b.HasIndex("PostId");

                    b.HasIndex("TagId");

                    b.ToTable("TagsRelation");
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.Email", b =>
                {
                    b.HasBaseType("AskPam.Crm.Conversations.Message");

                    b.Property<string>("Bcc");

                    b.Property<string>("Cc");

                    b.Property<string>("From");

                    b.Property<string>("Header");

                    b.Property<string>("HtmlBody");

                    b.Property<bool>("IsBodyHtml");

                    b.Property<bool>("IsReplied");

                    b.Property<string>("TextBody");

                    b.Property<string>("Thread");

                    b.Property<string>("To");

                    b.ToTable("Email");

                    b.HasDiscriminator().HasValue("Email");
                });

            modelBuilder.Entity("AskPam.Crm.Authorization.Followers.FollowersRelation", b =>
                {
                    b.HasOne("AskPam.Crm.Conversations.Conversation", "Conversation")
                        .WithMany("Followers")
                        .HasForeignKey("ConversationId");

                    b.HasOne("AskPam.Crm.Posts.Post", "Post")
                        .WithMany("Followers")
                        .HasForeignKey("PostId");

                    b.HasOne("AskPam.Crm.Authorization.User")
                        .WithMany("Followers")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("AskPam.Crm.Authorization.UserOrganization", b =>
                {
                    b.HasOne("AskPam.Crm.Organizations.Organization", "Organization")
                        .WithMany("Users")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AskPam.Crm.Authorization.User")
                        .WithMany("Organizations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AskPam.Crm.Authorization.UserRole", b =>
                {
                    b.HasOne("AskPam.Crm.Authorization.Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AskPam.Crm.Authorization.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AskPam.Crm.Configuration.Setting", b =>
                {
                    b.HasOne("AskPam.Crm.Organizations.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.HasOne("AskPam.Crm.Authorization.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("AskPam.Crm.Contacts.Contact", b =>
                {
                    b.HasOne("AskPam.Crm.Contacts.ContactGroup", "Group")
                        .WithMany("Contacts")
                        .HasForeignKey("GroupId");

                    b.HasOne("AskPam.Crm.Organizations.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("AskPam.Crm.Common.Address", "Address", b1 =>
                        {
                            b1.Property<long>("ContactId");

                            b1.Property<string>("Address1");

                            b1.Property<string>("Address2");

                            b1.Property<string>("City");

                            b1.Property<string>("Country");

                            b1.Property<string>("PostalCode");

                            b1.Property<string>("Province");

                            b1.ToTable("Contacts");

                            b1.HasOne("AskPam.Crm.Contacts.Contact")
                                .WithOne("Address")
                                .HasForeignKey("AskPam.Crm.Common.Address", "ContactId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("AskPam.Crm.Common.Phone", "BusinessPhone", b1 =>
                        {
                            b1.Property<long?>("ContactId");

                            b1.Property<string>("CountryCode");

                            b1.Property<string>("NationalFormat");

                            b1.Property<string>("Number")
                                .HasMaxLength(20);

                            b1.ToTable("Contacts");

                            b1.HasOne("AskPam.Crm.Contacts.Contact")
                                .WithOne("BusinessPhone")
                                .HasForeignKey("AskPam.Crm.Common.Phone", "ContactId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("AskPam.Crm.Common.Phone", "MobilePhone", b1 =>
                        {
                            b1.Property<long>("ContactId");

                            b1.Property<string>("CountryCode");

                            b1.Property<string>("NationalFormat");

                            b1.Property<string>("Number")
                                .HasMaxLength(20);

                            b1.ToTable("Contacts");

                            b1.HasOne("AskPam.Crm.Contacts.Contact")
                                .WithOne("MobilePhone")
                                .HasForeignKey("AskPam.Crm.Common.Phone", "ContactId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.Attachment", b =>
                {
                    b.HasOne("AskPam.Crm.Conversations.Message", "Message")
                        .WithMany("Attachments")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.Channel", b =>
                {
                    b.HasOne("AskPam.Crm.Contacts.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId");

                    b.HasOne("AskPam.Crm.Conversations.Conversation")
                        .WithMany("Channels")
                        .HasForeignKey("ConversationId");
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.Conversation", b =>
                {
                    b.HasOne("AskPam.Crm.Authorization.User", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToId");

                    b.HasOne("AskPam.Crm.Contacts.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId");

                    b.HasOne("AskPam.Crm.Organizations.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.DeliveryStatus", b =>
                {
                    b.HasOne("AskPam.Crm.Conversations.Message", "Message")
                        .WithMany("DeliveryStatus")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AskPam.Crm.Conversations.Message", b =>
                {
                    b.HasOne("AskPam.Crm.Conversations.Conversation", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AskPam.Crm.Authorization.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");
                });

            modelBuilder.Entity("AskPam.Crm.InternalNotes.InternalNote", b =>
                {
                    b.HasOne("AskPam.Crm.Contacts.Contact", "Contact")
                        .WithMany("InternalNotes")
                        .HasForeignKey("ContactId1");

                    b.HasOne("AskPam.Crm.Authorization.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("AskPam.Crm.Library.LibraryItem", "Library")
                        .WithMany()
                        .HasForeignKey("LibraryId");

                    b.HasOne("AskPam.Crm.Organizations.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AskPam.Crm.Posts.Post", "Post")
                        .WithMany("Notes")
                        .HasForeignKey("PostId1");
                });

            modelBuilder.Entity("AskPam.Crm.Library.LibraryItem", b =>
                {
                    b.HasOne("AskPam.Crm.Organizations.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AskPam.Crm.Notifications.Notification", b =>
                {
                    b.HasOne("AskPam.Crm.Authorization.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");
                });

            modelBuilder.Entity("AskPam.Crm.Notifications.UserNotification", b =>
                {
                    b.HasOne("AskPam.Crm.Notifications.Notification")
                        .WithMany("Users")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AskPam.Crm.Posts.Post", b =>
                {
                    b.HasOne("AskPam.Crm.Authorization.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");
                });

            modelBuilder.Entity("AskPam.Crm.Presence.ConnectedClient", b =>
                {
                    b.HasOne("AskPam.Crm.Authorization.User", "User")
                        .WithMany("ConnectedClients")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("AskPam.Crm.Stars.StarsRelation", b =>
                {
                    b.HasOne("AskPam.Crm.Contacts.Contact", "Contact")
                        .WithMany("StarRelations")
                        .HasForeignKey("ContactId");

                    b.HasOne("AskPam.Crm.Conversations.Conversation", "Conversation")
                        .WithMany("StarRelations")
                        .HasForeignKey("ConversationId");
                });

            modelBuilder.Entity("AskPam.Crm.Tags.Tag", b =>
                {
                    b.HasOne("AskPam.Crm.Organizations.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AskPam.Crm.Tags.TagsRelation", b =>
                {
                    b.HasOne("AskPam.Crm.Contacts.Contact", "Contact")
                        .WithMany("TagsRelations")
                        .HasForeignKey("ContactId");

                    b.HasOne("AskPam.Crm.Conversations.Conversation", "Conversation")
                        .WithMany("TagsRelations")
                        .HasForeignKey("ConversationId");

                    b.HasOne("AskPam.Crm.Library.LibraryItem", "Library")
                        .WithMany("TagsRelations")
                        .HasForeignKey("LibraryItemId");

                    b.HasOne("AskPam.Crm.Posts.Post", "Post")
                        .WithMany("Tags")
                        .HasForeignKey("PostId");

                    b.HasOne("AskPam.Crm.Tags.Tag", "Tag")
                        .WithMany("TagsRelations")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
