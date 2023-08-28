using AskPam.Crm.Authorization;
using AskPam.Exceptions;
using AskPam.Crm.Helpers;
using AskPam.Crm.Settings;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth0Nms = Auth0;
using System.Net;

namespace AskPam.Crm.Auth0
{
    public class Auth0UserService : IExternalUserService
    {
        private readonly AuthenticationApiClient _authentificationClient;
        private readonly Auth0Settings _auth0Settings;
        private IMemoryCache _cache;

        public Auth0UserService(IOptions<Auth0Settings> auth0Settings, IMemoryCache cache)
        {
            _auth0Settings = auth0Settings.Value;
            _authentificationClient = new AuthenticationApiClient(new Uri($"https://{_auth0Settings.Domain}"));
            _cache = cache;
        }

        public async Task<Token> AuthenticateUser(string email, string password)
        {
            try
            {
                var authenticationResponse = await _authentificationClient.AuthenticateAsync(new AuthenticationRequest()
                {
                    Connection = _auth0Settings.Connection,
                    ClientId = _auth0Settings.ClientId,
                    Scope = "openid email",
                    Username = email,
                    Password = password

                });

                var token = authenticationResponse.IdToken;

                return new Token
                {
                    IdToken = authenticationResponse.IdToken,
                    AccessToken = authenticationResponse.AccessToken,
                    RefreshToken = authenticationResponse.RefreshToken,
                    TokenType = authenticationResponse.TokenType,
                };
            }
            catch (Auth0Nms.Core.Exceptions.ApiException e)
            {
                throw new ApiException(e.Message, HttpStatusCode.BadRequest);
            }
        }
        public async Task<User> GetUserInfo(string token)
        {
            try
            {
                var response = await _authentificationClient.GetUserInfoAsync(token);
                return ConverToUser(response);
            }
            catch (Auth0Nms.Core.Exceptions.ApiException e)
            {
                throw new ApiException(e, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<User> GetUser(string email)
        {
            var managementClient = await GetManagementClient();
            var result = await managementClient.Users
                .GetAllAsync(q: "email : \"" + email.Trim().ToLower() + "\"");

            return result.Select(ConverToUser).FirstOrDefault();
        }

        public async Task<User> CreateUser(User user, bool emailVerified = true)
        {
            var managementClient = await GetManagementClient();

            //Create admin user for the tenant
            if (string.IsNullOrEmpty(user.Password))
            {
                user.SetRandomPassword();
            }

            var metadata = new
            {
                given_name = user.FirstName,
                family_name = user.LastName,
                name = $"{user.FirstName} {user.LastName}"
            };

            var newUser = await managementClient.Users.CreateAsync(new UserCreateRequest
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Connection = _auth0Settings.Connection,
                NickName = user.FullName,
                Email = user.Email,
                EmailVerified = emailVerified,
                Password = user.Password,
                UserMetadata = metadata
            }
            );
            user.Id = newUser.UserId;
            return user;


        }

        private async Task<ManagementApiClient> GetManagementClient()
        {
            var managementApiKey = await _cache.GetOrCreate("managementApiKey", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(24);
                return GenerateToken();
            });


            return new ManagementApiClient(
                managementApiKey,
                new Uri($"https://{_auth0Settings.Domain}/api/v2")
            );
        }

        private async Task<string> GenerateToken()
        {
            var _client = new RestClient($"https://{_auth0Settings.Domain}/oauth/token");

            var request = new RestRequest("", Method.POST);
            request.AddJsonBody(new
            {
                client_id = _auth0Settings.ManagementClientId,
                client_secret = _auth0Settings.ManagementClientSecret,
                audience = $"https://{_auth0Settings.Domain}/api/v2/",
                grant_type = "client_credentials"
            });

            request.AddHeader("Content-Type", "application/json");


            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

            RestRequestAsyncHandle handle = _client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            RestResponse response = (RestResponse)(await taskCompletion.Task);

            var ss = JsonConvert.DeserializeObject<dynamic>(response.Content);

            return ss.access_token;
        }

        public async Task ChangePassword(string userId, string password)
        {
            var managementClient = await GetManagementClient();

            try
            {
                await managementClient.Users.UpdateAsync(
                    userId,
                    new UserUpdateRequest
                    {
                        Password = password
                    }
                );
            }
            catch (Auth0Nms.Core.Exceptions.ApiException e)
            {
                throw new ApiException(e, HttpStatusCode.InternalServerError);
            }
        }
        public async Task ForgotPassword(string email)
        {
            try
            {
                var authenticationResponse = await _authentificationClient.ChangePasswordAsync(
                    new ChangePasswordRequest()
                    {
                        Connection = _auth0Settings.Connection,
                        ClientId = _auth0Settings.ClientId,
                        Email = email
                    }
                );

            }
            catch (Auth0Nms.Core.Exceptions.ApiException e)
            {
                throw new ApiException(e, HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateProfile(User user)
        {
            await UpdateUser(
                user,
                new UserUpdateRequest
                {
                    UserMetadata = new
                    {
                        given_name = user.FirstName,
                        family_name = user.LastName,
                        name = $"{user.FirstName} {user.LastName}"
                    }
                }
            );
        }

        public async Task UpdateProfilePicture(User user)
        {
            await UpdateUser(
                user,

                new UserUpdateRequest
                {
                    UserMetadata = new
                    {
                        picture = user.Picture,
                    }
                }
            );
        }

        public async Task RemoveProfilePicture(User user)
        {
            await UpdateUser(
                user,

                new UserUpdateRequest
                {
                    UserMetadata = new
                    {
                        picture = string.Empty,
                    }
                }
            );
        }

        public async Task<User> CreateUser(string firstname, string lastname, string email, string password = null)
        {
            var managementClient = await GetManagementClient();

            try
            {
                if (password == null)
                {
                    password = PasswordGenerator.GenerateRandomPassword();
                }

                var result = await managementClient.Users.CreateAsync(
                    new UserCreateRequest()
                    {
                        Email = email,
                        Password = password,
                        NickName = $"{firstname} {lastname}",
                        LastName = lastname,
                        FirstName = firstname,
                        Connection = _auth0Settings.Connection,
                        EmailVerified = true,
                        UserMetadata = new
                        {
                            given_name = firstname,
                            family_name = lastname,
                            name = $"{firstname} {lastname}"
                        }
                    }
                );

                return ConverToUser(result);
            }
            catch (Auth0Nms.Core.Exceptions.ApiException e)
            {
                throw new ApiException(e, HttpStatusCode.InternalServerError);
            }
        }

        #region Private

        private async Task UpdateUser(User user, UserUpdateRequest request)
        {
            var managementClient = await GetManagementClient();

            try
            {
                await managementClient.Users.UpdateAsync(
                    user.Id,
                    request
                );
            }
            catch (Auth0Nms.Core.Exceptions.ApiException e)
            {
                throw new ApiException(e);
            }
        }

        private User ConverToUser(Auth0Nms.Core.User user)
        {
            try
            {
                string userId = user.UserId;

                string firstName = user.UserMetadata["given_name"];
                string lastName = user.UserMetadata["family_name"];

                string picture = string.IsNullOrEmpty(user.UserMetadata["picture"]?.Value) ? user.Picture : user.UserMetadata["picture"];

                return new User
                (
                    user.UserId,
                    firstName,
                    lastName,
                    user.Email,
                    picture
                );
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + ' ' + JsonConvert.SerializeObject(user));
            }
        }
        #endregion
    }

}
