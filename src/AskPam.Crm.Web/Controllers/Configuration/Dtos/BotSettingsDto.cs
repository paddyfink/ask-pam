using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Configuration.Dtos
{
    public class BotSettingsDto
    {
        [Required]
        public string BotName { get; set; }
        [Required]
        public string BotAvatar { get; set; }
        [Required]
        public string Intro { get; set; }
        [Required]
        public string Outro { get; set; }
        [Required]
        public bool? BotEnabled { get; set; }
        [Required]
        public bool? DesactivationEnabled { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Certainty threshold must be between 0 and 100")]
        public double? Treshold { get; set; }
    }
}
