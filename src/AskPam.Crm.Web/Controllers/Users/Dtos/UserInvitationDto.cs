using AskPam.Crm.Common.Dtos;
using AskPam.Application.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace AskPam.Crm.Users.Dtos
{
    public class UserInvitationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
