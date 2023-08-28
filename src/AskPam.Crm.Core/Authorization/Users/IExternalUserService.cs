using System.Threading.Tasks;

namespace AskPam.Crm.Authorization
{
    public interface IExternalUserService
    {
        Task<Token> AuthenticateUser(string email, string password);
        Task<User> GetUserInfo(string accessToken);
        Task ChangePassword(string userId, string password);
        Task UpdateProfile(User user);
        Task UpdateProfilePicture(User user);
        Task<User> CreateUser(string firstname, string lastname, string email, string password = null);
        Task ForgotPassword(string email);
        Task<User> GetUser(string email);
        Task RemoveProfilePicture(User user);
    }
}
