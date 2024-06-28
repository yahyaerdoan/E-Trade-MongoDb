using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using MongoDb.UserInterface.GoogleCloudStorage.Utilities.ConfigOptions;

namespace MongoDb.UserInterface.GoogleCloudStorage.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly GoogleCloudStorageConfigOptions _options;
        private readonly GoogleCredential _googleCredential;

        public CloudStorageService(IOptions<GoogleCloudStorageConfigOptions> options)
        {
            _options = options.Value;
            var environment = Environment.GetEnvironmentVariable("GCPStorageAuthFile");
            _googleCredential = environment == Environments.Production
                ? GoogleCredential.FromJson(_options.GCPStorageAuthFile)
                : GoogleCredential.FromFile(_options.GCPStorageAuthFile);
        }

        public async Task<string> UploadFileAsync(IFormFile fileToUpload, string fileNameToSave)
        {
            using (var memoryStream = new MemoryStream())
            {
                await fileToUpload.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using (var storageClient = StorageClient.Create(_googleCredential))
                {
                    var uploadedFile = await storageClient.UploadObjectAsync(
                        _options.GoogleCloudStorageBucketName,
                        fileNameToSave,
                        fileToUpload.ContentType,
                        memoryStream);
                    return uploadedFile.MediaLink;
                }
            }
        }

        public async Task<string> GetSignedUrlAsync(string fileNameToRead, int timeOutInMinutes = 30)
        {
            var sac = _googleCredential.UnderlyingCredential as ServiceAccountCredential;
            var urlSigner = UrlSigner.FromServiceAccountCredential(sac);

            var signedUrl = await urlSigner.SignAsync(_options.GoogleCloudStorageBucketName, fileNameToRead, TimeSpan.FromMinutes(timeOutInMinutes));
            return signedUrl.ToString();
        }

        public async Task DeleteFileAsync(string fileNameToDelete)
        {
            using (var storageClient = StorageClient.Create(_googleCredential))
            {
                await storageClient.DeleteObjectAsync(_options.GoogleCloudStorageBucketName, fileNameToDelete);
            }
        }
    }
}
