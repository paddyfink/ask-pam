//using Microsoft.AspNetCore.TestHost;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;
//using Xunit;

//namespace AskPam.Crm.Tests.IntegrationTests
//{
//    [Collection(Consts.TEST_COLLECTION)]
//    public abstract class BaseIntegrationTest: IClassFixture<TestServerFixture>
//    {
//        public readonly HttpClient client;
//        public readonly TestServer server;
//        // here we get our testServerFixture, also see above IClassFixture.
//        protected BaseIntegrationTest(TestServerFixture testServerFixture)
//        {
//            client = testServerFixture.client;
//            server = testServerFixture.server;
//        }
//    }
//}
