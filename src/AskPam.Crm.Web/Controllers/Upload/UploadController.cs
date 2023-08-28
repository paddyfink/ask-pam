using AskPam.Crm.Authorization;
using AskPam.Crm.Controllers.Authorization.Dtos;
using AskPam.Exceptions;
using AskPam.Crm.Runtime.Session;
using AskPam.Crm.Storage;
using AskPam.Crm.Users.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Upload
{
    [Route("/api/upload")]
    public class UploadController : BaseController
    {
        private readonly IStorageService _storageService;
        private readonly IUserManager _userManager;

        public UploadController(
            ICrmSession session,
            IMapper mapper,
            IStorageService storageService,
            IUserManager userManager
        ) : base(session, mapper)
        {
            _storageService = storageService;
            _userManager = userManager;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ProfilePictureDto), 200)]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            EnsureOrganization();

            var user = await _userManager.FindByIdAsync(Session.UserId);

            FileValidation(file);

            using (Stream stream = file.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var fileContent = binaryReader.ReadBytes((int)file.Length);
                    var url = await _storageService.SaveProfilePicture(fileContent, file.FileName, user);
                    await _userManager.UpdateProfilePicture(user, url);
                }
            }

            return new ObjectResult(Mapper.Map<User, ProfilePictureDto>(user));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ProfilePictureDto), 200)]
        public async Task<IActionResult> ResetProfilePicture()
        {
            EnsureOrganization();

            var user = await _userManager.FindByIdAsync(Session.UserId);

            await _storageService.RemoveProfilePicture(user);
            user = await _userManager.ResetProfilePicture(user);

            return new ObjectResult(Mapper.Map<User, ProfilePictureDto>(user));
        }

        #region Private

        private static void FileValidation(IFormFile file)
        {
            if (file == null) throw new ApiException("File can't be empty.");
            if (file.Length == 0) throw new ApiException("File can't be empty.");
        }
        #endregion
    }
}
