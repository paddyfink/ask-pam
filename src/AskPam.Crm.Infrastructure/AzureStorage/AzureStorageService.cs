using System.IO;
using System.Threading.Tasks;
using AskPam.Crm.Authorization;
using AskPam.Crm.Configuration;
using AskPam.Crm.Conversations;
using AskPam.Crm.Settings;
using AskPam.Crm.Storage;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AskPam.Crm.AzureStorage
{
    public class AzureStorageService : IStorageService
    {
        private readonly ISettingManager _settingManager;

        public AzureStorageService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }
        public async Task<string> AddFile(byte[] file, string fileName, Conversation conv)
        {
            fileName = fileName.Replace("|", string.Empty);

            // Retrieve reference to a previously created container.
            var path = string.Format(await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.AzureStorage.ConversationAttachmentPath),
                conv.OrganizationId.ToString(), conv.Id.ToString(), fileName).ToLower();

            return await SaveFile(file, fileName, path);
        }

        public async Task<string> SaveProfilePicture(byte[] file, string fileName, User user)
        {
            return await SaveFile(file, fileName, await GetProfilePicturePath(user, fileName));
        }

        public async Task RemoveProfilePicture(User user)
        {
            await RemoveFileIfExist(await GetProfilePicturePath(user, user.Picture));
        }

        #region Private

        private async Task<string> GetProfilePicturePath(User user, string currentPath)
        {
            //exemple: auth0|588b7ec5aa03fb78b0e55ca5
            var userId = user.Id.Replace("|", string.Empty);

            var extension = Path.GetExtension(currentPath);

            // Retrieve reference to a previously created container. (Account/Pictures/{0}{1})
            return string.Format(await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.AzureStorage.AvatarPath), userId, extension).ToLower();
        }

        private async Task<string> SaveFile(byte[] file, string fileName, string path)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.AzureStorage.ConnectionString));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.AzureStorage.Container));
            if (await container.CreateIfNotExistsAsync())
            {
                await container.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    }
                );
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(path);

            using (Stream stream = new MemoryStream(file, false))
            {
                await blockBlob.UploadFromStreamAsync(stream);
            }
            return blockBlob.Uri.AbsoluteUri;
        }

        private async Task RemoveFileIfExist(string path)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.AzureStorage.ConnectionString));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.AzureStorage.Container));
            if (await container.ExistsAsync())
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(path);
                await blockBlob.DeleteIfExistsAsync();
            }
        }

        #endregion

    }
}
