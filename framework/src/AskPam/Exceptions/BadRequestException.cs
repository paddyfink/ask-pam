using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AskPam.Exceptions
{
    public class BadRequestException : ApiException
    {
        public List<string> Errors { get; set; }

        public BadRequestException(string message)
            : base(message ,HttpStatusCode.BadRequest)
        {
        }
    }
}
