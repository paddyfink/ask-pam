using System;
using System.Collections.Generic;
using System.Text;
using AskPam.Crm.Authorization.Users;
using AskPam.Crm.Contacts;
using AskPam.Crm.Controllers.Common;
using AskPam.Crm.Conversations;
using AskPam.Crm.InternalNotes;
using AskPam.Crm.Library;
using AskPam.Crm.Organizations;
using AutoMapper;
using AutoMapper.Configuration;

namespace AskPam.Crm.UnitTests.Controllers
{
    public class AutoMapperFixture : IDisposable
    {
        public AutoMapperFixture()
        {
            var mappings = new MapperConfigurationExpression();

            mappings.AddProfile<ConversationsAutomapperProfile>();
            mappings.AddProfile<ContactsAutomapperProfile>();
            mappings.AddProfile<CommonAutoMapperProfile>();
            mappings.AddProfile<UserAutomapperProfile>();
            mappings.AddProfile<LibraryAutomapperProfile>();
            mappings.AddProfile<OrganizationsAutomapperProfile>();
            mappings.AddProfile<InternalNotesAutomapperProfile>();
            

            Mapper.Initialize(mappings);
        }

        public void Dispose()
        {
            
        }
    }
}
