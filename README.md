Theory Questions 


# GitHub Link: 
https://github.com/st10449059/CLDV6211-POE-St10449059


# YouTube link: 
https://youtu.be/VukVhs6QfwI

# App Running
# Home Page:  
<img width="939" height="500" alt="image" src="https://github.com/user-attachments/assets/0558dcf8-92aa-41e1-82c6-5c11ad8965d4" />


# Event Page:
<img width="939" height="500" alt="image" src="https://github.com/user-attachments/assets/98e7f7f4-5767-4621-8ff1-d5bbe2481c9f" />


# Venues Page:
<img width="939" height="498" alt="image" src="https://github.com/user-attachments/assets/dace1406-5b25-4ee3-a45b-e86092895a61" />


# Bookings Page:
<img width="938" height="502" alt="image" src="https://github.com/user-attachments/assets/89635c2d-989d-4fc2-9ac2-0d5b84e69f5d" />



# Azure Storage Explorer
<img width="939" height="497" alt="image" src="https://github.com/user-attachments/assets/b136d2bf-9269-4c86-a51d-bd8728ccff2f" />

# Blob service code 

using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

/* * CODE ATTRIBUTION & REFERENCE:
 * The implementation of Azure Blob Storage integration and the Azurite compatibility 
 * fixes were developed using official Microsoft documentation.
 * * Microsoft. (2023). Azure Blob storage client library for .NET. 
 * Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/
 */

namespace CLDV6211_Assignment_Part_1_St10449059.Services
{
    /// <summary>
    /// Service responsible for handling file uploads to Azure Blob Storage or local Azurite emulator.
    /// This service supports Phase A (Cloud Data Management) requirements.
    /// </summary>
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(string connectionString)
        {
            /* * PART 2: COMPATIBILITY FIX
             * We force a specific older API version (2021-08-06) to ensure 
             * compatibility with the lab's specific version of the Azurite emulator.
             * This prevents "Version Not Supported" errors during development.
             */
            var options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2021_08_06);
            _blobServiceClient = new BlobServiceClient(connectionString, options);
        }

        /// <summary>
        /// Uploads a file from an HTML form to a specified blob container.
        /// </summary>
        /// <param name="file">The IFormFile received from the controller.</param>
        /// <param name="containerName">Target container (e.g., 'venue-images' or 'event-images').</param>
        /// <returns>The public URI of the uploaded blob.</returns>
        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            // Initialize the container client using the service client
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            try
            {
                /*
                 * Ensures the container exists in the storage environment.
                 * Set to PublicAccessType.Blob to allow the web application to display 
                 * images directly via their URL.
                 */
                await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }
            catch (Azure.RequestFailedException)
            {
                // Silently continue if container check fails due to version handshake issues with Azurite
            }

            /*
             * Generate a unique blob name using Path.GetRandomFileName() to avoid 
             * overwriting files with the same name in the storage container.
             */
            var blobClient = containerClient.GetBlobClient(Path.GetRandomFileName() + Path.GetExtension(file.FileName));

            // Open the file stream for reading the uploaded content
            using var stream = file.OpenReadStream();

            /* * PART 2: UPLOAD FIX
             * Performs an asynchronous upload. The 'true' parameter allows overwriting
             * if a blob with the same name happens to exist.
             */
            await blobClient.UploadAsync(stream, true);

            // Return the absolute string URL of the blob for database persistence
            return blobClient.Uri.ToString();
        }
    }
}

# Refrences 
•	Connolly, T. M. & Begg, C. E. (2015). Database Systems: A Practical Approach to Design, Implementation, and Management. 6th edition. Pearson Education.

•	Coyne, J. (2021). CSS Refactoring: Architecting Systems for Change. 2nd edition. O'Reilly Media.

•	Elmasri, R. & Navathe, S. B. (2017). Fundamentals of Database Systems. 7th edition. Pearson.

•	Freeman, A. (2022). Pro ASP.NET Core 6: Develop Cloud-Ready Web Applications Using MVC, Blazor, and Razor Pages. 9th edition. Apress.

•	Lerman, J. & Miller, R. (2015). Programming Entity Framework: DbContext. 2nd edition. O'Reilly Media.

•	Lucid Software Inc. (2026). Lucidchart Cloud-based Visual Workspace. [Online]. Available at: https://www.lucidchart.com [Accessed 14 April 2026].

•	Microsoft. (2023). ASP.NET Core Middleware. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/ [Accessed 14 April 2026].

•	Microsoft. (2023). Configuration in ASP.NET Core. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/ [Accessed 14 April 2026].

•	Microsoft. (2023). Model-View-Controller (MVC) Pattern. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview [Accessed 14 April 2026].

•	Microsoft. (2023). Primary and Foreign Key Constraints. [Online]. Available at: https://learn.microsoft.com/en-us/sql/relational-databases/tables/primary-and-foreign-key-constraints [Accessed 14 April 2026].

• CelerData (2025). Normalization vs denormalization: The trade-offs you need to know. [online] Available at: https://celerdata.com/glossary/normalization-vs-denormalization-the-trade-offs-you-need-to-know [Accessed 3 May 2026].

• Codd, E.F. (1970). A Relational Model of Data for Large Shared Data Banks. Communications of the ACM, [online] 13(6), pp. 377-387. Available at: https://doi.org/10.1145/362384.362685 [Accessed 4 May 2026].

•Couchbase (2025). Data normalization vs. denormalization comparison. [online] The Couchbase Blog. Available at: https://www.couchbase.com/blog/normalization-vs-denormalization/ [Accessed 2 May 2026].

• Google Cloud (2026). What is database normalization? [online] Available at: https://cloud.google.com/discover/what-is-database-normalization [Accessed 5 May 2026].

• IBM (2026). What is database normalization? [online] Available at: https://www.ibm.com/think/topics/database-normalization [Accessed 6 May 2026].

• Imaginary Cloud (2025). Azure AI Search: Benefits, use cases and implementation. [online] Available at: https://www.imaginarycloud.com/blog/azure-ai-search-enterprise-guide/ [Accessed 30 April 2026].

• Microsoft Learn (2026a). Introduction to Azure AI Search. [online] Available at: https://learn.microsoft.com/en-us/azure/search/search-what-is-azure-search [Accessed 1 May 2026].

• Microsoft Learn (2026b). Vector search overview - Azure AI Search. [online] Available at: https://learn.microsoft.com/en-us/azure/search/vector-search-overview [Accessed 1 May 2026].

• Microsoft Learn (2026c). Azure Blob storage client library for .NET. [online] Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/ [Accessed 4 May 2026].

• Microsoft Learn (2026d). Handle concurrency exceptions in Entity Framework Core. [online] Available at: https://learn.microsoft.com/en-us/ef/core/saving/concurrency [Accessed 5 May 2026].

• Plain Concepts (2026). Azure Cognitive Search | Introduction. [online] Available at: https://www.plainconcepts.com/azure-cognitive-search-introduction/ [Accessed 2 May 2026].

• ScaleGrid (2025). What is database normalization. [online] Available at: https://scalegrid.io/blog/what-is-database-normalization/ [Accessed 6 May 2026].

• Unstructured (2025). Comparing vector and keyword search for AI applications. [online] Available at: https://unstructured.io/insights/comparing-vector-and-keyword-search-for-ai-applications [Accessed 4 May 2026].




