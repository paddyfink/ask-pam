using AskPam.Application.Dto;


namespace AskPam.Crm.Authorization.Dtos
{
    public class ProfileDto //: FullAuditedDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
    }
}
