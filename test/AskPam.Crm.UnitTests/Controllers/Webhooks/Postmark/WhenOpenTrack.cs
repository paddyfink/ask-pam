using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Conversations;
using AskPam.Crm.Conversations.Dtos;
using AskPam.Crm.Conversations.Dtos.PostMark;
using AskPam.Crm.EntityFramework;
using AskPam.Crm.Organizations;
using AskPam.Crm.Runtime.Session;
using AskPam.Crm.UnitTests.TestData;
using AskPam.Crm.WebHooks;
using AskPam.EntityFramework.Repositories;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers.Webhooks.Postmark
{
    public class WhenOpenTrack : BasePostMarkController
    {
        public WhenOpenTrack()
        {
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1);
        }


        [Fact(DisplayName = "WhenOpenTrack_ShouldCreateNewClienDeliveryStatus")]
        public async Task ShouldAddNewDeliveryStatus()
        {
            var postMarkMessageId = Guid.NewGuid();
            var conversation = await Context.Conversations.Include(c => c.Messages).ThenInclude(m => m.DeliveryStatus).FirstAsync();
            var message = conversation.Messages.First();
            message.PostmarkId = postMarkMessageId;
            Context.Conversations.Update(conversation);
            await Context.SaveChangesAsync();
            await _controller.OpenTrack(JsonConvert.DeserializeObject<OpenedEmail>(MessageContent.OpenTrackJson.Replace("{0}", postMarkMessageId.ToString())));

            conversation = await Context.Conversations.Include(c => c.Messages).ThenInclude(m => m.DeliveryStatus).FirstAsync();
            var deliveryStatus = conversation.Messages.First().DeliveryStatus.First();

            conversation.Messages.First().DeliveryStatus.Count.ShouldBe(1);
            conversation.LastLocation.City.ShouldBe("Novi Sad");
            deliveryStatus.Open.ShouldBeTrue();

        }

        [Fact(DisplayName = "WhenOpenTrack_ShouldUpdateExistingStatus")]
        public async Task ShouldUpdateExistingStatus()
        {
            var postMarkMessageId = Guid.NewGuid();
            var conversation = await Context.Conversations.Include(c => c.Messages).ThenInclude(m => m.DeliveryStatus).FirstAsync();
            var message = conversation.Messages.First();
            message.PostmarkId = postMarkMessageId;
            message.DeliveryStatus.Add(new DeliveryStatus { ChannelTypeId = ChannelType.Email.Value, Open = false, Date = new DateTime() });
            Context.Conversations.Update(conversation);
            await Context.SaveChangesAsync();


            await _controller.OpenTrack(JsonConvert.DeserializeObject<OpenedEmail>(MessageContent.OpenTrackJson.Replace("{0}", postMarkMessageId.ToString())));

            conversation = await Context.Conversations.Include(c => c.Messages).ThenInclude(m => m.DeliveryStatus).FirstAsync();
            var deliveryStatus = conversation.Messages.First().DeliveryStatus.First();
            conversation.Messages.First().DeliveryStatus.Count.ShouldBe(1);
            conversation.LastLocation.City.ShouldBe("Novi Sad");
            deliveryStatus.Open.ShouldBeTrue();

        }

        [Fact(DisplayName = "WhenOpenTrack_WhenExistingStatusForOtherChannel_ShouldAddNewDeliveryStatus")]
        public async Task WhenExistingStatusForOtherChannel_ShouldAddNewDeliveryStatus()
        {
            var postMarkMessageId = Guid.NewGuid();
            var conversation = await Context.Conversations.Include(c => c.Messages).ThenInclude(m => m.DeliveryStatus).FirstAsync();
            var message = conversation.Messages.First();
            message.PostmarkId = postMarkMessageId;
            message.DeliveryStatus.Add(new DeliveryStatus { ChannelTypeId = ChannelType.Messenger.Value, Open = false, Date = new DateTime() });
            Context.Conversations.Update(conversation);
            await Context.SaveChangesAsync();


            await _controller.OpenTrack(JsonConvert.DeserializeObject<OpenedEmail>(MessageContent.OpenTrackJson.Replace("{0}", postMarkMessageId.ToString())));

            conversation = await Context.Conversations.Include(c => c.Messages).ThenInclude(m => m.DeliveryStatus).FirstAsync();
            var deliveryStatus = conversation.Messages.First().DeliveryStatus.First(c => c.ChannelTypeId == ChannelType.Email.Value);
            conversation.Messages.First().DeliveryStatus.Count.ShouldBe(2);
            deliveryStatus.Open.ShouldBeTrue();

        }
    }
}
