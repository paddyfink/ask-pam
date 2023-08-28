using AskPam.Crm.Common.Dtos;
using AskPam.Application.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace AskPam.Crm.Users.Dtos
{
    public class UserRoleDto
    {
        public int Id { get; set; }
        public Guid? OrganizationId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsStatic { get; set; }
        public bool IsDefault { get; set; }
    }
}
