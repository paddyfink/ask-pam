using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AskPam.Crm.Helpers
{
    public static class StringHelper
    {
        public static string TextToHtml(string text)
        {
            text = WebUtility.HtmlEncode(text);
            text = text.Replace("\r\n", "\r");
            text = text.Replace("\n", "\r");
            text = text.Replace("\r", "<br>\r\n");
            text = text.Replace("  ", " &nbsp;");

            return text;
        }
    }
}
