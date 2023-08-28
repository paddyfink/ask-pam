using AskPam.Crm.Common.Dtos;
using AskPam.Application.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace AskPam.Crm.InternalNotes.Dtos
{
    public class NoteDto
    {
        public int? Id { get; set; }

        //Personal Information
        public string Comment { get; set; }

        //Settings
        public DateTime? CreatedAt { get; set; }

        //Foreign Keys + Tables
        public string CreatedById { get; set; }
        public string CreatedByFullName { get; set; }
        public string CreatedByPicture { get; set; }
        public int? ContactId { get; set; }
        public int? PostId { get; set; }

    }
}
