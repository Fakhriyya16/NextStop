
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Service.Configurations;
using Service.Interfaces;

namespace Service
{
    public class CloudManagement : ICloudManagement
    {
        private readonly Cloudinary _cloudinary;

        public CloudManagement(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<(string Url, string PublicId)> UploadImageWithPublicIdAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "NextStop"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Image upload failed");
            }

            return (uploadResult.SecureUrl.AbsoluteUri, uploadResult.PublicId);
        }

        public async Task DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Image deletion failed");
            }
        }
    }
}
