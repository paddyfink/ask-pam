using AskPam.Crm.Common.Dtos;
using AskPam.Application.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace AskPam.Crm.Users.Dtos
{
    public class UserDto : FullAuditedDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public bool? EmailVerified { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
