using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CLDV6211_Assignment_Part_1_St10449059.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient; // Fixed name
        private const string ContainerName = "venue-images";

        public BlobService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString); // Fixed name
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(Path.GetRandomFileName() + Path.GetExtension(file.FileName));

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString();
        }
    }
}