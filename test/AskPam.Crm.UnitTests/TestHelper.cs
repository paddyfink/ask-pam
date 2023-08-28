using AskPam.Crm.Authorization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using AskPam.Crm.Integrations;
using AskPam.Crm.Organizations;
using AskPam.Crm.Runtime.Session;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AskPam.Crm.UnitTests
{

    public static class TestHelper
    {

        public const string User1Id = "user1";
        public const string User2Id = "user2";
        public const string User3Id = "user3";
        public const string User4Id = "user4";
        public const string User5Id = "user5";

        public const string Orgnization1Id = "6689db00-2744-41c9-9c93-e459114f7fb4";
        public const string Orgnization2Id = "a5dd18df-bb73-483e-9ba8-a33e9d09c90e";

        public static List<Organization> Organizations => new List<Organization> {
            new Organization { Id = new Guid(Orgnization1Id), Name = "Organization 1"} ,
            new Organization { Id = new Guid(Orgnization2Id), Name = "Organization 2" }
        };

        public static List<User> Users => new List<User> {
            new User(User1Id, "user", "1", "user1@test.com"),
            new User(User2Id, "user", "2", "user2@test.com"),
            new User(User3Id, "user", "3", "user3@test.com"),
            new User(User4Id, "user", "4", "user4@test.com"),
        };

        public static void GenerateGlobalDataTests(DbContextOptions<CrmDbContext> contextOptions)
        {
            using (var context = new CrmDbContext(contextOptions, new NullCrmSession()))
            {
                context.Organizations.AddRange(Organizations);
                context.Users.AddRange(Users);
                context.SaveChanges();
            }
        }

        public static void GenerateContactDataTests(DbContextOptions<CrmDbContext> contextOptions, Guid orgnizationID, int contactCount = 10)
        {
            using (var context = new CrmDbContext(contextOptions, new NullCrmSession()))
            {
                var groups = new Faker<ContactGroup>()
                    .CustomInstantiator(f => new ContactGroup(f.Random.AlphaNumeric(10), orgnizationID))
                       .Generate(contactCount);
                context.ContactGroups.AddRange(groups);

                context.Contacts.AddRange(new Faker<Contact>()
                    .CustomInstantiator(f => new Contact(f.Name.FirstName(), f.Name.LastName(), orgnizationID))
                    .Generate(contactCount)
                    );

                context.SaveChanges();
            }
        }

        public static void GenerateConversationsdataTest(DbContextOptions<CrmDbContext> contextOptions,
            Guid organizationId,
            int conversationsCount = 50,
            int messagesCount = 50,
            bool botDisabled = false,
            bool isArchived = false,
            bool isFlagged = false)
        {
            using (var context = new CrmDbContext(contextOptions, new NullCrmSession()))
            {

                context.Conversations.AddRange(new Faker<Conversation>()
                    .CustomInstantiator(f => new Conversation(organizationId,
                    f.Name.FullName(),
                    seen: f.Random.Bool(),
                    botDisabled: botDisabled,
                    isArchived: isArchived,
                    isFlagged: isFlagged))
                    .Generate(conversationsCount));
                context.SaveChanges();

                foreach (var conv in context.Conversations.Where(c => c.OrganizationId == organizationId).ToList())
                {

                    var messages = new Faker<Message>()
                    .RuleFor(m => m.MessageType, f => f.PickRandom(MessageType.GetAll()))
                    .RuleFor(m => m.Status, f => f.PickRandom(MessageStatus.GetAll()))
                    .Generate(messagesCount);
                    foreach (var message in messages)
                    {
                        conv.AddMessage(message);
                    }

                    context.SaveChanges();
                }
            }
        }

    }
}
