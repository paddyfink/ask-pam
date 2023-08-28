namespace AskPam.Crm.UnitTests.TestData
{
    public static class MessageContent
    {
        #region Email
        public const string IncomeMessageJson = @"{
          ""FromName"": ""John Doe"",
          ""From"": ""john.doe@email.com"",
          ""FromFull"": {
            ""Email"": ""john.doe@email.com"",
            ""Name"": ""John Doe"",
            ""MailboxHash"": """"
          },
          ""To"": ""\""org@dev.askpam.co\"" <org@dev.askpam.co>"",
          ""ToFull"": [
            {
              ""Email"": ""org@dev.askpam.co"",
              ""Name"": ""org@dev.askpam.co"",
              ""MailboxHash"": """"
            }
          ],
          ""Cc"": """",
          ""CcFull"": [],
          ""Bcc"": """",
          ""BccFull"": [],
          ""OriginalRecipient"": ""org@dev.askpam.co"",
          ""Subject"": ""test"",
          ""MessageID"": ""675ac552-475b-4116-b34d-112e9ec5609b"",
          ""ReplyTo"": """",
          ""MailboxHash"": """",
          ""Date"": ""Tue, 4 Apr 2017 14:51:57 +0000"",
          ""TextBody"": ""Test email\r\n"",
          ""HtmlBody"": ""<html xmlns:v=\""urn:schemas-microsoft-com:vml\"" xmlns:o=\""urn:schemas-microsoft-com:office:office\"" xmlns:w=\""urn:schemas-microsoft-com:office:word\"" xmlns:m=\""http:\/\/schemas.microsoft.com\/office\/2004\/12\/omml\"" xmlns=\""http:\/\/www.w3.org\/TR\/REC-html40\"">\r\n<head>\r\n<meta http-equiv=\""Content-Type\"" content=\""text\/html; charset=us-ascii\"">\r\n<meta name=\""Generator\"" content=\""Microsoft Word 15 (filtered medium)\"">\r\n<style><!--\r\n\/* Font Definitions *\/\r\n@font-face\r\n\t{font-family:\""Cambria Math\"";\r\n\tpanose-1:2 4 5 3 5 4 6 3 2 4;}\r\n@font-face\r\n\t{font-family:Calibri;\r\n\tpanose-1:2 15 5 2 2 2 4 3 2 4;}\r\n\/* Style Definitions *\/\r\np.MsoNormal, li.MsoNormal, div.MsoNormal\r\n\t{margin:0cm;\r\n\tmargin-bottom:.0001pt;\r\n\tfont-size:11.0pt;\r\n\tfont-family:\""Calibri\"",sans-serif;\r\n\tmso-fareast-language:EN-US;}\r\na:link, span.MsoHyperlink\r\n\t{mso-style-priority:99;\r\n\tcolor:#0563C1;\r\n\ttext-decoration:underline;}\r\na:visited, span.MsoHyperlinkFollowed\r\n\t{mso-style-priority:99;\r\n\tcolor:#954F72;\r\n\ttext-decoration:underline;}\r\nspan.EmailStyle17\r\n\t{mso-style-type:personal-compose;\r\n\tfont-family:\""Calibri\"",sans-serif;\r\n\tcolor:windowtext;}\r\n.MsoChpDefault\r\n\t{mso-style-type:export-only;\r\n\tfont-family:\""Calibri\"",sans-serif;\r\n\tmso-fareast-language:EN-US;}\r\n@page WordSection1\r\n\t{size:612.0pt 792.0pt;\r\n\tmargin:70.85pt 70.85pt 70.85pt 70.85pt;}\r\ndiv.WordSection1\r\n\t{page:WordSection1;}\r\n--><\/style><!--[if gte mso 9]><xml>\r\n<o:shapedefaults v:ext=\""edit\"" spidmax=\""1026\"" \/>\r\n<\/xml><![endif]--><!--[if gte mso 9]><xml>\r\n<o:shapelayout v:ext=\""edit\"">\r\n<o:idmap v:ext=\""edit\"" data=\""1\"" \/>\r\n<\/o:shapelayout><\/xml><![endif]-->\r\n<\/head>\r\n<body lang=\""FR\"" link=\""#0563C1\"" vlink=\""#954F72\"">\r\n<div class=\""WordSection1\"">\r\n<p class=\""MsoNormal\""><span lang=\""EN-CA\"">Test email<o:p><\/o:p><\/span><\/p>\r\n<\/div>\r\n<\/body>\r\n<\/html>\r\n"",
          ""StrippedTextReply"": """",
          ""Tag"": """",
          ""Headers"": [
            {
              ""Name"": ""Received"",
              ""Value"": ""by inbound.postmarkapp.com (Postfix, from userid 993)id 9E90F1A0ADC; Tue,  4 Apr 2017 14:52:02 +0000 (UTC)""
            },
            {
              ""Name"": ""X-Spam-Checker-Version"",
              ""Value"": ""SpamAssassin 3.4.0 (2014-02-07) onp-pm-inbound03-pktewr1""
            },
            {
              ""Name"": ""X-Spam-Status"",
              ""Value"": ""No""
            },
            {
              ""Name"": ""X-Spam-Score"",
              ""Value"": ""-0.0""
            },
            {
              ""Name"": ""X-Spam-Tests"",
              ""Value"": ""DKIM_SIGNED,DKIM_VALID,HTML_MESSAGE,MIME_HTML_MOSTLY,RCVD_IN_DNSWL_NONE,RCVD_IN_MSPIKE_H3,RCVD_IN_MSPIKE_WL,SPF_HELO_PASS,SPF_PASS""
            },
            {
              ""Name"": ""Received-SPF"",
              ""Value"": ""Pass (sender SPF authorized) identity=mailfrom; client-ip=104.47.42.114; helo=nam03-by2-obe.outbound.protection.outlook.com; envelope-from=paddy@ask-pam.com; receiver={0}@dev.askpam.co""
            },
            {
              ""Name"": ""Received"",
              ""Value"": ""from NAM03-BY2-obe.outbound.protection.outlook.com (mail-by2nam03on0114.outbound.protection.outlook.com [104.47.42.114])(using TLSv1.2 with cipher ECDHE-RSA-AES256-SHA384 (256\/256 bits))(No client certificate requested)by inbound.postmarkapp.com (Postfix) with ESMTPS id DC58D1A0AC2for <{0}@dev.askpam.co>; Tue,  4 Apr 2017 10:52:00 -0400 (EDT)""
            },
            {
              ""Name"": ""DKIM-Signature"",
              ""Value"": ""v=1; a=rsa-sha256; c=relaxed\/relaxed; d=pamalexander.onmicrosoft.com; s=selector1-askpam-com0i; h=From:Date:Subject:Message-ID:Content-Type:MIME-Version; bh=A7NwBWCDC4Y6aOx8icvE387RYHYE4EDnAyiX8uts48w=; b=jpmd6uuUGGdG+7f+dqwoEf6brK0szbgTT3R1PRNUg1\/NjxOGZMTpuq44DajQ42UbJlGXKIXzhIhOBQ\/sZYv+CTOrA9hdOC428puG2yctW6IhK+UE0JiD4hrIpMdXxbCyKw3hrOwSFd7Bg4Pn78vztmzyc9M3V99QXVtiax8GUEs=""
            },
            {
              ""Name"": ""Received"",
              ""Value"": ""from BY2PR01MB425.prod.exchangelabs.com (10.141.140.14) by BY2PR01MB426.prod.exchangelabs.com (10.141.140.18) with Microsoft SMTP Server (version=TLS1_2, cipher=TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA256_P256) id 15.1.1005.10; Tue, 4 Apr 2017 14:51:58 +0000""
            },
            {
              ""Name"": ""Received"",
              ""Value"": ""from BY2PR01MB425.prod.exchangelabs.com ([10.141.140.14]) by BY2PR01MB425.prod.exchangelabs.com ([10.141.140.14]) with mapi id 15.01.1005.019; Tue, 4 Apr 2017 14:51:58 +0000""
            },
            {
              ""Name"": ""Thread-Topic"",
              ""Value"": ""test""
            },
            {
              ""Name"": ""Thread-Index"",
              ""Value"": ""AdKtUvyNIOH2qeL\/RfyDnHMSLMhjTQ==""
            },
            {
              ""Name"": ""Message-ID"",
              ""Value"": ""<BY2PR01MB425511D03454CB192D0E78AF50B0@BY2PR01MB425.prod.exchangelabs.com>""
            },
            {
              ""Name"": ""Accept-Language"",
              ""Value"": ""en-US""
            },
            {
              ""Name"": ""Content-Language"",
              ""Value"": ""en-US""
            },
            {
              ""Name"": ""X-MS-Has-Attach"",
              ""Value"": """"
            },
            {
              ""Name"": ""X-MS-TNEF-Correlator"",
              ""Value"": """"
            },
            {
              ""Name"": ""authentication-results"",
              ""Value"": ""dev.askpam.co; dkim=none (message not signed) header.d=none;dev.askpam.co; dmarc=none action=none header.from=ask-pam.com;""
            },
            {
              ""Name"": ""x-originating-ip"",
              ""Value"": ""[207.107.69.35]""
            },
            {
              ""Name"": ""x-microsoft-exchange-diagnostics"",
              ""Value"": ""1;BY2PR01MB426;6:4aNpqS0ozTBDd9CIoWnsJ5Cmd3npeqcdN+EH7TYu8ZyfkWVc+r7OiX729qVaNVfWEPDZ\/5CEp4a8rMxBrzWM7bYziGWQD6Ckj+YKvZ\/hblm6f0idYEamPPbPsI6Su5DbSo157bVJqiRPl65OEciB+j76wo4tnpeyB\/\/dhSerrdhytq9ekxEkF5AHPapZAqWTdeThDP\/Cxq9b2HIVaUj\/z2PhdXs4b+glu3WxRb0MFfgAg93smZfbHQB1p+fIwg7xExtYW+gKGG1DB5MfY0xieZ0whcnfoj2SobWRq8SFQe4=;5:ZDbHDK8sdRtnQGWcPl5wA6CJXCSjxkQ+op7gs7SEXb+kYXd5slny30XuSswMZw+k0uXXhsX+CZ1hN0vNdHKI5paAasolypPR4XjKSwzNZBQkfdhYHzC+yIho6ZWW4VIH5wEhVqWn3K9yfUwmIvef1Q==;24:qIq3p4JqRJsqQj5\/\/7hHw6iL\/2DI6mMfeFpv2bzMLOM6l4eUbAhW+\/WvHZvdVoe6o\/zcXvJaenrZaVADnGL4xhJSAsCIOFIT1ZJBZGAlY9k=;7:n0\/W86GTn3k7VIn0f64pQP8qsM5PHnsVBpULeCfBo2l9YrGsPZkjDhTAvkLVqCErFxM0tK8vm8HSFRO+2OwBvq\/u9pptnU3xXWNjARPbySRZZIkoZJXI6+3lgQOgrTxRzY\/6VW3I8IAecPv7PVYXAe2myGhCsdMSCKZo7BscdnUCmtBuatz+WpOAlySVD73u5JP60FnjL\/xpGGNvWjR414W16OzEd9wEKC5JTD9wBtY3Vwy+TW4pTfHQeYjmP5xOge378gn9NvQS6w+cYk\/zJlyC35BTM1HUtQXeTCrss5Oel7FP1ycKAvZ8LFVAEk0plOed7wNe0vT87uBM+UH6wQ==""
            },
            {
              ""Name"": ""x-ms-office365-filtering-correlation-id"",
              ""Value"": ""5e352a66-47a3-4dc6-b7e6-08d47b6a24e8""
            },
            {
              ""Name"": ""x-microsoft-antispam"",
              ""Value"": ""UriScan:;BCL:0;PCL:0;RULEID:(22001)(2017030254075)(201703131423075);SRVR:BY2PR01MB426;""
            },
            {
              ""Name"": ""x-microsoft-antispam-prvs"",
              ""Value"": ""<BY2PR01MB426F2F42462A67F4B425985F50B0@BY2PR01MB426.prod.exchangelabs.com>""
            },
            {
              ""Name"": ""x-exchange-antispam-report-test"",
              ""Value"": ""UriScan:(21748063052155);""
            },
            {
              ""Name"": ""x-exchange-antispam-report-cfa-test"",
              ""Value"": ""BCL:0;PCL:0;RULEID:(6040450)(2401047)(8121501046)(5005006)(3002001)(10201501046)(93006095)(93001095)(6041248)(20161123562025)(201703131423075)(201702281528075)(201703061421075)(2016111802025)(20161123560025)(20161123555025)(20161123564025)(6072148)(6043046);SRVR:BY2PR01MB426;BCL:0;PCL:0;RULEID:;SRVR:BY2PR01MB426;""
            },
            {
              ""Name"": ""x-forefront-prvs"",
              ""Value"": ""0267E514F9""
            },
            {
              ""Name"": ""x-forefront-antispam-report"",
              ""Value"": ""SFV:NSPM;SFS:(10019020)(6009001)(39410400002)(39400400002)(39830400002)(39450400003)(555874004)(74316002)(7696004)(5660300001)(122556002)(7116003)(221733001)(4270600005)(2906002)(2351001)(3280700002)(3660700001)(189998001)(110136004)(2900100001)(38730400002)(33656002)(558084003)(80792005)(77096006)(25786009)(7736002)(66066001)(2501003)(3480700004)(8936002)(54356999)(81166006)(8676002)(588024002)(6436002)(53936002)(50986999)(6916009)(5640700003)(6506006)(99286003)(54896002)(55016002)(6306002)(9686003)(3846002)(102836003)(6116002)(86362001)(790700001)(42262002)(217283001)(220243001);DIR:OUT;SFP:1102;SCL:1;SRVR:BY2PR01MB426;H:BY2PR01MB425.prod.exchangelabs.com;FPR:;SPF:None;MLV:sfv;LANG:en;""
            },
            {
              ""Name"": ""spamdiagnosticoutput"",
              ""Value"": ""1:99""
            },
            {
              ""Name"": ""spamdiagnosticmetadata"",
              ""Value"": ""NSPM""
            },
            {
              ""Name"": ""MIME-Version"",
              ""Value"": ""1.0""
            },
            {
              ""Name"": ""X-OriginatorOrg"",
              ""Value"": ""ask-pam.com""
            },
            {
              ""Name"": ""X-MS-Exchange-CrossTenant-originalarrivaltime"",
              ""Value"": ""04 Apr 2017 14:51:57.9471 (UTC)""
            },
            {
              ""Name"": ""X-MS-Exchange-CrossTenant-fromentityheader"",
              ""Value"": ""Hosted""
            },
            {
              ""Name"": ""X-MS-Exchange-CrossTenant-id"",
              ""Value"": ""b0342d0e-14d5-4296-907a-6ce72ee5fcad""
            },
            {
              ""Name"": ""X-MS-Exchange-Transport-CrossTenantHeadersStamped"",
              ""Value"": ""BY2PR01MB426""
            }
          ],
          ""Attachments"": []
        }";

        public const string OpenTrackJson = @"
            {
                ""FirstOpen"": true,
                ""Client"": {
                ""Name"": ""Chrome 35.0.1916.153"",
                ""Company"": ""Google"",
                ""Family"": ""Chrome""
                },
                ""OS"": {
                ""Name"": ""OS X 10.7 Lion"",
                ""Company"": ""Apple Computer, Inc."",
                ""Family"": ""OS X 10""
                },
                ""Platform"": ""WebMail"",
                ""UserAgent"": ""Mozilla\/5.0 (Macintosh; Intel Mac OS X 10_7_5) AppleWebKit\/537.36 (KHTML, like Gecko) Chrome\/35.0.1916.153 Safari\/537.36"",
                ""ReadSeconds"": 5,
                ""Geo"": {
                ""CountryISOCode"": ""RS"",
                ""Country"": ""Serbia"",
                ""RegionISOCode"": ""VO"",
                ""Region"": ""Autonomna Pokrajina Vojvodina"",
                ""City"": ""Novi Sad"",
                ""Zip"": ""21000"",
                ""Coords"": ""45.2517,19.8369"",
                ""IP"": ""188.2.95.4""
                },
                ""MessageID"": ""{0}"",
                ""ReceivedAt"": ""2017-12-06T13:46:30-05:00"",
                ""Tag"": ""welcome-email"",
                ""Recipient"": ""john@example.com""
            }
";

        public const string BounceJson = @"
         {
            ""ID"": 42,
            ""Type"": ""HardBounce"",
            ""TypeCode"": 1,
            ""Name"": ""Hard bounce"",
            ""Tag"": ""Test"",
            ""MessageID"": ""{0}"",
            ""ServerID"": 23,
            ""Description"": ""The server was unable to deliver your message (ex: unknown user, mailbox not found)."",
            ""Details"": ""Test bounce details"",
            ""Email"": ""john@example.com"",
            ""From"": ""sender@example.com"",
            ""BouncedAt"": ""2014-08-01T13:28:10.2735393-04:00"",
            ""DumpAvailable"": true,
            ""Inactive"": true,
            ""CanActivate"": true,
            ""Subject"": ""Test subject"",
            ""Content"": ""<Full dump of bounce>"",
            }
        ";
        #endregion

        #region Smooch
        public const string SmoochJson = @"
            {
                ""trigger"": ""message:appUser"",
                ""app"": {
                    ""_id"": ""{0}""
                },
                ""messages"": [
                    {
                        ""text"": ""This is a text message"",
                        ""type"": ""text"",
                        ""role"": ""appUser"",
                        ""received"": 1489098741.848,
                        ""authorId"": ""eb1ee9757cbefe1682d11001"",
                        ""name"": ""John Doe"",
                        ""_id"": ""58c1d7f5f5743052005a5b6b"",
                        ""source"": {
                            ""type"": ""web"",
                            ""id"": ""1f62f3be3f604f2cbe696c5bf26037d0""
                        }
            }
                ],
                ""appUser"": {
                    ""_id"": ""eb1ee9757cbefe1682d11001"",
                    ""surname"": ""New"",
                    ""givenName"": ""Message"",
                    ""signedUpAt"": ""2016-05-11T16:27:36.120Z"",
                    ""conversationStarted"": true,
                    ""credentialRequired"": false,
                    ""devices"": [
                        {
                            ""lastSeen"": ""2017-03-01T02:13:49.112Z"",
                            ""info"": {
                                ""currentTitle"": ""Ask PAM | Home"",
                                ""currentUrl"": ""http://askpamweb.azurewebsites.net/"",
                                ""browserLanguage"": ""en-US"",
                                ""referrer"": """",
                                ""userAgent"": ""Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"",
                                ""URL"": ""askpamweb.azurewebsites.net"",
                                ""sdkVersion"": ""3.13.1""
                            },
                            ""platform"": ""web"",
                            ""id"": ""6c1ac026f46840999bbf7210f8812617"",
                            ""active"": true
                        },
                        {
                            ""lastSeen"": ""2017-03-09T22:32:21.848Z"",
                            ""info"": {
                                ""currentTitle"": ""Ask PAM | Home"",
                                ""currentUrl"": ""http://askpamweb.azurewebsites.net/"",
                                ""browserLanguage"": ""en-GB"",
                                ""referrer"": """",
                                ""userAgent"": ""Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3033.0 Safari/537.36"",
                                ""URL"": ""askpamweb.azurewebsites.net"",
                                ""sdkVersion"": ""3.13.2""
                            },
                            ""platform"": ""web"",
                            ""id"": ""1f62f3be3f604f2cbe696c5bf26037d0"",
                            ""active"": true
                        },
                        {
                            ""displayName"": ""+1 514-884-6632"",
                            ""lastSeen"": ""2017-03-01T02:08:46.111Z"",
                            ""info"": {
                                ""phoneNumber"": ""+15148846632"",
                                ""country"": null,
                                ""city"": null,
                                ""state"": null
                            },
                            ""id"": ""e4c3e42c-a61b-402c-b317-838d46b888b7"",
                            ""platform"": ""twilio"",
                            ""linkedAt"": ""2017-02-06T23:06:13.094Z"",
                            ""active"": true
                        },
                        {
                            ""info"": {
                                ""avatarUrl"": ""https://scontent.xx.fbcdn.net/v/t31.0-1/16423111_10155009667037248_7949788882208788168_o.jpg?oh=2ff8b3580d33adacb94989d8af843796&oe=59332133""
                            },
                            ""avatarUrl"": ""https://scontent.xx.fbcdn.net/v/t31.0-1/16423111_10155009667037248_7949788882208788168_o.jpg?oh=2ff8b3580d33adacb94989d8af843796&oe=59332133"",
                            ""displayName"": ""Paddy Finken"",
                            ""id"": ""e34d1e55-e4ff-48dc-abd2-a11bb9f0bd54"",
                            ""platform"": ""messenger"",
                            ""linkedAt"": ""2017-02-23T18:25:20.482Z"",
                            ""lastSeen"": ""2017-03-01T02:09:40.371Z"",
                            ""active"": true
                        }
                    ],
                    ""clients"": [
                        {
                            ""lastSeen"": ""2017-03-01T02:13:49.112Z"",
                            ""info"": {
                                ""currentTitle"": ""Ask PAM | Home"",
                                ""currentUrl"": ""http://askpamweb.azurewebsites.net/"",
                                ""browserLanguage"": ""en-US"",
                                ""referrer"": """",
                                ""userAgent"": ""Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"",
                                ""URL"": ""askpamweb.azurewebsites.net"",
                                ""sdkVersion"": ""3.13.1""
                            },
                            ""platform"": ""web"",
                            ""id"": ""6c1ac026f46840999bbf7210f8812617"",
                            ""active"": true
                        },
                        {
                            ""lastSeen"": ""2017-03-09T22:32:21.848Z"",
                            ""info"": {
                                ""currentTitle"": ""Ask PAM | Home"",
                                ""currentUrl"": ""http://askpamweb.azurewebsites.net/"",
                                ""browserLanguage"": ""en-GB"",
                                ""referrer"": """",
                                ""userAgent"": ""Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3033.0 Safari/537.36"",
                                ""URL"": ""askpamweb.azurewebsites.net"",
                                ""sdkVersion"": ""3.13.2""
                            },
                            ""platform"": ""web"",
                            ""id"": ""1f62f3be3f604f2cbe696c5bf26037d0"",
                            ""active"": true
                        },
                        {
                            ""displayName"": ""+1 514-884-6632"",
                            ""lastSeen"": ""2017-03-01T02:08:46.111Z"",
                            ""info"": {
                                ""phoneNumber"": ""+15148846632"",
                                ""country"": null,
                                ""city"": null,
                                ""state"": null
                            },
                            ""id"": ""e4c3e42c-a61b-402c-b317-838d46b888b7"",
                            ""platform"": ""twilio"",
                            ""linkedAt"": ""2017-02-06T23:06:13.094Z"",
                            ""active"": true
                        },
                        {
                            ""info"": {
                                ""avatarUrl"": ""https://scontent.xx.fbcdn.net/v/t31.0-1/16423111_10155009667037248_7949788882208788168_o.jpg?oh=2ff8b3580d33adacb94989d8af843796&oe=59332133""
                            },
                            ""avatarUrl"": ""https://scontent.xx.fbcdn.net/v/t31.0-1/16423111_10155009667037248_7949788882208788168_o.jpg?oh=2ff8b3580d33adacb94989d8af843796&oe=59332133"",
                            ""displayName"": ""Paddy Finken"",
                            ""id"": ""e34d1e55-e4ff-48dc-abd2-a11bb9f0bd54"",
                            ""platform"": ""messenger"",
                            ""linkedAt"": ""2017-02-23T18:25:20.482Z"",
                            ""lastSeen"": ""2017-03-01T02:09:40.371Z"",
                            ""active"": true
                        }
                    ],
                    ""pendingClients"": [],
                    ""properties"": {}
                }
            }
";
        #endregion
    }
}
