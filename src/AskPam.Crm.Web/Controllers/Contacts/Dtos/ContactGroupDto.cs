using AskPam.Crm.Common.Dtos;
using AskPam.Application.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace AskPam.Crm.Contacts.Dtos
{
    public class ContactGroupDto : FullAuditedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ContactsCount { get; set; }
    }
}
