using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AskPam.Crm.Common
{
    public static class Util
    {
        public static bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            return ((int)statusCode >= 200) && ((int)statusCode <= 299);
        }
    }
}
