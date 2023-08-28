//using AskPam.Crm.AI;
//using AskPam.Crm.Configuration;
//using AskPam.Crm.Contacts;
//using AskPam.Crm.Conversations;
//using AskPam.Crm.Followers;
//using AskPam.Events;
//using AskPam.Crm.Organizations;
//using AskPam.Crm.Settings;
//using AskPam.Crm.Storage;
//using AskPam.Domain.Repositories;
//using Microsoft.Extensions.Options;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace AskPam.Crm.UnitTests.Conversations
//{
//    public class ConversationsManager_Test
//    {
//        private Mock<IRepository<Conversation>> mockConversationRepository;
//        private Mock<ISmoochConversationService> mocksmoochConversationService;
//        private Mock<IRepository<Message>> mockmessageRepository;
//        private Mock<IRepository<Conversation>> mockconversationsRepository;
//        private Mock<IRepository<Organization, Guid>> mockorganizationRepository;
//        private Mock<IDomainEvents> mockdomainEvents;
//        private Mock<IStorageService> mockstorageService;
//        private Mock<IEmailService> mockpostmarkEMailService;
//        private Mock<IOptions<AppSettings>> mockappSettings;
//        private Mock<ContactManager> mockcontactManager;
//        private Mock<FollowersManager> mockfollowersManager;
//        private Mock<SettingManager> mocksettingManager;
//        private Mock<QnAMakerManager> mockqnAMakerManager;
//        private Mock<IRepository<Channel>> mockChannelsRepository;
//        private ConversationManager conversationsManager;

//        public ConversationsManager_Test()
//        {
//            conversationsManager = new ConversationManager(
//                mockmessageRepository.Object,
//                mockConversationRepository.Object,
//                mockdomainEvents.Object,
//                mocksmoochConversationService.Object,
//                mockstorageService.Object,
//                mockpostmarkEMailService.Object,
//                mockcontactManager.Object,
//                mockfollowersManager.Object,
//                mocksettingManager.Object,
//                mockqnAMakerManager.Object,
//                mockappSettings.Object,
//                mockorganizationRepository.Object,
//                mockChannelsRepository.Object);
//        }

//        //[Fact]
//        //public WhenReceiveNewMessage()
//        //{

//        //}
//    }
//}
