﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Settings
{
    public class Auth0Settings
    {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string Connection { get; set; }
        public string ClientSecret { get; set; }
        public string ManagementClientId { get; set; }
        public string ManagementClientSecret { get; set; }
        
    }
}
