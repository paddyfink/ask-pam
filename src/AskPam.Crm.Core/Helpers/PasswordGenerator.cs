using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Helpers
{
    public static class PasswordGenerator
    {
        public static string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 16);
        }
    }
}
