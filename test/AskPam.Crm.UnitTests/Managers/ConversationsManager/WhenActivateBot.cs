using AskPam.Crm.EntityFramework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AskPam.Crm.UnitTests.Managers.Conversations
{
    public class WhenActivateBot : BaseConversationManagerTests
    {
        public WhenActivateBot()
        {
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }


        [Fact(DisplayName = "WhenActivateBot_ShouldActivateBot")]
        public async Task ShouldActivateBot()
        {
            // Arrange
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1, botDisabled: true);
            var conversation = await Context.Conversations.FindAsync((long)1);
            var conversationManager = CreateManager();

            // Act
            await conversationManager.ActivateBot(conversation);

            // Assert
            (await Context.Conversations.FindAsync((long)1)).BotDisabled.ShouldBe(false);
        }

        [Fact(DisplayName = "WhenActivateBot_ShouldDeActivateBot()")]
        public async Task ShouldDeActivateBot()
        {
            // Arrange
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1, botDisabled: false);
            var conversation = await Context.Conversations.FindAsync((long)1);
            var conversationManager = CreateManager();

            // Act
            await conversationManager.ActivateBot(conversation);

            // Assert
            (await Context.Conversations.FindAsync((long)1)).BotDisabled.ShouldBe(true);
        }

    }
}
