using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Authorization
{
    public class Token
    {
        public string IdToken { get; set; }

        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public string RefreshToken { get; set; }
    }
}
