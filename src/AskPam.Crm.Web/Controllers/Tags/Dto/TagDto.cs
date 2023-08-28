using System.ComponentModel.DataAnnotations;

namespace AskPam.Crm.Tags.Dtos
{
    public class TagDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public string FullName { get; set; }
    }
}
