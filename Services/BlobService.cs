using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CLDV6211_Assignment_Part_1_St10449059.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            try
            {
                // Ensures the container exists in the cloud and sets public access for viewing
                await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }
            catch (Azure.RequestFailedException)
            {
                // Fail silently if it already exists
            }

            // Generate unique blob name
            var blobClient = containerClient.GetBlobClient(Path.GetRandomFileName() + Path.GetExtension(file.FileName));

            using var stream = file.OpenReadStream();

            // Upload to Azure
            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString();
        }

        // ==========================================
        // This is the missing method your controller is looking for!
        // ==========================================
        public async Task<bool> DeleteFileAsync(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl)) return false;

            try
            {
                // Extract the blob name from the complete Azure URL string
                var uri = new System.Uri(blobUrl);
                var blobName = System.IO.Path.GetFileName(uri.LocalPath);

                // Target the container (extracting the folder name from the URL)
                var segments = uri.Segments;
                if (segments.Length < 3) return false;

                string containerName = segments[segments.Length - 2].Trim('/');
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                // Delete the image from Azure Storage
                return await blobClient.DeleteIfExistsAsync();
            }
            catch
            {
                // Fail silently so database transaction isn't broken by a storage warning
                return false;
            }
        }
    }
}