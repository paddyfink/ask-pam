using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers.Conversations
{
   public  class WhenFlag : ConversationsControllerTests
    {
        public WhenFlag()
        {
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
            CurrentUser = TestHelper.Users.Find(u => u.Id == TestHelper.User1Id);
        }
                
        [Fact(DisplayName = "WhenFlag_ShouldCallConversationManager")]
        public async Task ShouldCallConversationManager()
        {
         // Arrange  
            var controller = CreateConversationsController();
            var orgId = new Guid(TestHelper.Orgnization1Id);            
            var conversation = new Conversation(orgId,"conv", "1");
                        
            MockConversationManager.Setup(d => d.FindByIdAsync(1, orgId)).ReturnsAsync(conversation);
            MockUserManager.Setup(u => u.FindByIdAsync(TestHelper.User1Id)).ReturnsAsync(CurrentUser);
            
            // Act
            await controller.Flag(1);

            //Assert
            MockConversationManager.Verify(d => d.Flag(conversation, CurrentUser), Times.AtLeastOnce());

        }
    }
}
