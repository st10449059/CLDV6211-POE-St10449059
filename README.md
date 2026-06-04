# Theory questions
Reflective Technical Report
List of Application Features I created EventEase, a complete event and venue management system, for this project. Among the primary elements I put in place are:

•	A dynamic events dashboard with a sophisticated filtering system that lets users look up events by date, place, and category.

•	A strong venue booking system that expressly forbids multiple booking on the same day by using backend concurrency validation.

•	A media upload system that integrates with the cloud to attach banner pictures to newly created events.

•	A normalized relational database structure that uses lookup tables instead of hardcoded variables to link events, venues, and event categories.

The Experience and Difficulties of Migration One of the most difficult yet instructive aspects of the project were transferring this application from my local development environment to the actual Azure cloud. Initially, I used the Azurite storage emulator and SQL LocalDB to develop and test everything.

To replace my local connection strings for the live Azure SQL and Blob Storage endpoints, the appsettings.json file must be properly updated during the migration process. Managing Entity Framework database migrations across the two environments was one difficulty I encountered during this shift. I momentarily returned to my local database for testing after changing my models and deploying to the cloud. The new ImageUrl column was missing from my local database, which caused the application to crash and throw an "Invalid column name" SQL exception. To get my environments properly synchronized again, I had to use the Package Manager Console to drop and rebuild the local database.

This experience served as a real illustration of the importance of keeping development and production environments apart. I was able to safely debug and rebuild my local database without ever jeopardizing the integrity of the live cloud application by keeping them apart.

Technologies Used and Component Discussion I used three essential Azure services to make sure the application was secure and scalable in the cloud:

•	Azure App Service: I hosted the web application on this platform. I was able to deploy the ASP.NET Core project straight from Visual Studio without needing to set up a server or virtual machine by hand.

•	Azure SQL Database: Because the system mostly uses relational data, I decided to use this to manage the data layer. Strict data integrity and transactional locking are necessary to prevent double bookings, and relational SQL databases are ideal for these tasks.

•	Azure Blob Storage: I incorporated Blob Storage rather than storing large image files straight into the SQL database, which would result in database bloat and slow down queries. Only the lightweight image URL string is saved to the database by the program, which uploads the actual image file to the blob container.

To guarantee a clear division between the database logic and the user interface, the project's foundation was constructed using C# and ASP.NET Core MVC. I controlled all version control through GitHub to make sure my codebase was secure before publishing anything to production, and I utilized Entity Framework Core to communicate with the database using object-oriented LINQ queries rather than raw SQL strings. 

# Advanced Cloud Theory

1. Cosmos DB versus Traditional Databases
Azure Cosmos DB's emphasis on flexible schema management and horizontal scalability sets it apart from conventional relational database management systems. Conventional databases rely on vertical scaling and a strict schema, which necessitates updating server hardware to accommodate growing demands. Cosmos DB, on the other hand, enables a multi-model approach that includes wide column, document, key value, and graph formats.

To guarantee rigorous consistency, traditional relational databases give priority to ACID features within a centralized architecture. Because Cosmos DB runs in distributed contexts, developers can use the CAP theorem to balance consistency, availability, and partition tolerance (Babucea, 2021). Because of this, Cosmos DB is appropriate for large-scale applications that need low latency access to unstructured data, but traditional databases are still the best choice for structured transaction processing where defined relationships are needed (Paci, 2022).

2. Security Considerations for Logic Apps
•	Security must be incorporated into the architecture when creating Azure Logic Apps that handle sensitive data. Important factors include:

•	Identity and Access Management: Logic Apps can be authenticated with other Azure services by using Managed Identities. As a result, embedded credentials are no longer required (Morrow, 2013).

•	Data Encryption: Both in transit and at rest, sensitive data must be encrypted. Although Azure offers automatic encryption, developers should manage encryption keys with Key Vault.

•	Network Isolation: To guarantee that the Logic App environment is cut off from the public internet, use private endpoints and VNET integration (Arif, 2025).

•	Logging and Auditing: Turn on diagnostic logging to keep an eye on how the workflow is being carried out. This guarantees adherence to data protection laws and enables the tracking of unwanted access attempts.

3. Creating Robust Workflows with Event Grid
By separating event producers from event consumers, Azure Event Grid establishes reliable workflows. This event-driven architecture improves system efficiency by enabling services to respond to state changes without constant polling (Yu & Buyya, 2009).

Organizations may create automated procedures that successfully manage failures by connecting Event Grid with services like Azure Functions or Logic Apps. Dead letter queues and built-in retry rules are supported by Event Grid. The system transfers a message to a storage container for further examination or reprocessing if it is unable to reach its intended recipient. When individual components encounter unavailability or processing spikes, this mix of services guarantees that the workflow stays functional and fault-tolerant (Sobral, 2026).



# GitHub link 
https://github.com/st10449059/CLDV6211-POE-St10449059
# YouTube link 
https://youtu.be/JgpI0LujRhY
# Application link 
https://eventease-app-yk-g6gfg6ffcgcuhkc8.southafricanorth-01.azurewebsites.net/
PLEASE NOTE FOR ASSESSOR: To strictly comply with the Section C rubric requirement which dictates that "Resources are dropped, and proof is provided," the Azure Resource Group hosting this application was safely deleted after final testing. Consequently, this live URL is no longer active.
Full, indisputable proof that the web application was fully accessible via the Azure URL, loaded quickly without startup errors, and successfully executed all features (filtering, database querying, and blob storage uploads) in the live production environment is provided in the YouTube Video Demonstration and the screenshot evidence below.


 
# Screenshots 
 
Home Page 
<img width="939" height="500" alt="image" src="https://github.com/user-attachments/assets/2965523c-257e-4bbf-b34d-d86d17ff01f0" />

 
Venues Page
<img width="939" height="499" alt="image" src="https://github.com/user-attachments/assets/f9259cd3-972a-4b5d-97f9-6ef5c2419054" />

 
Events Page
<img width="939" height="503" alt="image" src="https://github.com/user-attachments/assets/f6f31080-766f-419b-979a-99fc662e9c5f" />

 
Bookings Page 
<img width="939" height="500" alt="image" src="https://github.com/user-attachments/assets/fd26e0cd-50dd-4459-a2e6-c759a6ce794f" />

 
Proof that I have dropped all my resources.
<img width="939" height="505" alt="image" src="https://github.com/user-attachments/assets/5408cca3-1512-4f36-92c3-c841f3310042" />


 
Storage Browser image.
<img width="939" height="514" alt="image" src="https://github.com/user-attachments/assets/fd0bfdc2-cf4a-4d51-a68f-dd052bbfc5d1" />



Code attributions and references 
Events controller:

/* * =========================================================================================
 * CODE ATTRIBUTION
 * =========================================================================================
 * Description: Advanced Data Filtering & LINQ Queries
 * Source: Microsoft (2023) Complex Querying in Entity Framework Core.
 * Link: https://learn.microsoft.com/en-us/ef/core/querying/
 * * Description: Concurrency & Validation (Double-booking logic)
 * Source: Microsoft (2023) Handle concurrency conflicts in EF Core.
 * Link: https://learn.microsoft.com/en-us/ef/core/saving/concurrency
 * =========================================================================================
 */

using CLDV6211_Assignment_Part_1_St10449059.Data;
using CLDV6211_Assignment_Part_1_St10449059.Models;
using CLDV6211_Assignment_Part_1_St10449059.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CLDV6211_Assignment_Part_1_St10449059.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobService _blobService;

        public EventsController(ApplicationDbContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // ==========================================
        // Advanced Filter Index Method
        // ==========================================
        public async Task<IActionResult> Index(int? eventTypeId, int? venueId, System.DateTime? startDate, System.DateTime? endDate)
        {
            // Populate lookup tables for the frontend filters
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventTypeId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", venueId);

            // Base query with eager loading for relational data
            var query = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            // Apply dynamic LINQ filters based on user selection
            if (eventTypeId.HasValue)
            {
                query = query.Where(e => e.EventTypeId == eventTypeId);
            }
            if (venueId.HasValue)
            {
                query = query.Where(e => e.VenueId == venueId);
            }
            if (startDate.HasValue)
            {
                query = query.Where(e => e.EventDate >= startDate);
            }
            if (endDate.HasValue)
            {
                query = query.Where(e => e.EventDate <= endDate);
            }

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDate,VenueId,EventTypeId")] Event @event, IFormFile imageFile)
        {
            // Concurrency Validation: Prevents double-booking the same venue on the same day --
            var isAlreadyBooked = await _context.Events.AnyAsync(e =>
                e.VenueId == @event.VenueId &&
                e.EventDate.Date == @event.EventDate.Date);

            if (isAlreadyBooked)
            {
                ModelState.AddModelError("", "Validation Error: This venue is already occupied on the selected date.");
                ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
                ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
                return View(@event);
            }

            if (ModelState.IsValid)
            {
                // Upload image to Azure Blob Storage if provided --
                if (imageFile != null)
                {
                    @event.ImageUrl = await _blobService.UploadFileAsync(imageFile, "event-images");
                }
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,VenueId,EventTypeId,ImageUrl")] Event @event, IFormFile? imageFile)
        {
            if (id != @event.EventId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        @event.ImageUrl = await _blobService.UploadFileAsync(imageFile, "event-images");
                    }
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Validation: Ensure events with active attendee bookings cannot be deleted
            var hasActiveBookings = await _context.Bookings.AnyAsync(b => b.EventId == id);

            if (hasActiveBookings)
            {
                TempData["Error"] = "Validation Error: Cannot delete an event that has active attendee bookings.";
                return RedirectToAction(nameof(Index));
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                // Isolate and delete the associated image from Azure Blob Storage
                if (!string.IsNullOrEmpty(@event.ImageUrl))
                {
                    await _blobService.DeleteFileAsync(@event.ImageUrl);
                }

                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id) => _context.Events.Any(e => e.EventId == id);
    }
}

 
Programming class: 
/* * =========================================================================================
 * CODE ATTRIBUTION
 * =========================================================================================
 * Description: Configuration of the web application builder and middleware pipeline.
 * Source: Microsoft (2023) ASP.NET Core Fundamentals.
 * Link: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/startup
 * * Description: Dependency injection in ASP.NET Core for SQL and Blob Services.
 * Source: Microsoft (2023) Dependency injection in ASP.NET Core.
 * Link: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
 * =========================================================================================
 */

using Microsoft.EntityFrameworkCore;
using CLDV6211_Assignment_Part_1_St10449059.Data;
using CLDV6211_Assignment_Part_1_St10449059.Services;

namespace CLDV6211_Assignment_Part_1_St10449059
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Database Service Registration: 
            // Registering the ApplicationDbContext into the Dependency Injection (DI) container. 
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // PART 2: Azure Blob Storage Service Registration
            // We retrieve the "AzureBlobStorage" connection string from appsettings.json
            // and register the BlobService as a Singleton for use across the application.
            var blobConnection = builder.Configuration.GetConnectionString("AzureBlobStorage");
            builder.Services.AddSingleton(x => new BlobService(blobConnection));

            // Registers the MVC services required to handle Controllers and Views.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // 2. Middleware Pipeline Configuration:
            // Defines how HTTP requests are handled as they travel through the application.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Enables the serving of static assets like CSS, JavaScript, and Images.
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // 3. Routing:
            // Defines the Conventional Routing pattern used to map URLs to actions.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

/* * =========================================================================================
 * REFERENCE LIST & ATTRIBUTION 
 * =========================================================================================
 *   Connolly, T. M. & Begg, C. E. (2015). Database Systems: A Practical Approach. 6th ed.
 *   Coyne, J. (2021). CSS Refactoring: Architecting Systems for Change. 2nd edition.
 *   Elmasri, R. & Navathe, S. B. (2017). Fundamentals of Database Systems. 7th edition.
 *   Freeman, A. (2022). Pro ASP.NET Core 6. 9th edition. Apress.
 *   Lerman, J. & Miller, R. (2015). Programming Entity Framework: DbContext. 2nd edition.
 *   Lucid Software Inc. (2026). Lucidchart Cloud-based Visual Workspace. [Online].
 *   Microsoft. (2023). ASP.NET Core Middleware. [Online].
 *   Microsoft. (2023). Configuration in ASP.NET Core. [Online].
 *   Microsoft. (2023). Model-View-Controller (MVC) Pattern. [Online].
 *   Microsoft. (2023). Primary and Foreign Key Constraints. [Online].
 *   Microsoft. (2023). Azure Blob storage documentation. [Online]. Available at: 
 * https://learn.microsoft.com/en-us/azure/storage/blobs/ [Accessed 02 June 2026].
 *   Microsoft. (2023). Dependency injection in ASP.NET Core. [Online]. Available at:
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
 *   Microsoft. (2023). Complex Querying in Entity Framework Core. [Online]. Available at: 
 * https://learn.microsoft.com/en-us/ef/core/querying/ [Accessed 02 June 2026].
 *   Microsoft. (2023). Handle concurrency conflicts in EF Core. [Online]. Available at: 
 * https://learn.microsoft.com/en-us/ef/core/saving/concurrency [Accessed 02 June 2026].
 * =========================================================================================
 */

Blob services:
/* * =========================================================================================
 * CODE ATTRIBUTION
 * =========================================================================================
 * Description: Azure Blob Storage Integration (Upload & Delete Async Methods)
 * Source: Microsoft (2023) Azure Blob storage client library v12 for .NET.
 * Link: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
 * =========================================================================================
 */

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

# References
•	Freeman, A. (2022). Pro ASP.NET Core 6. 9th edition. Apress.

•	Microsoft. (2023). Overview of ASP.NET Core MVC. Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview 

•	Microsoft. (2023). App Service documentation. Available at: https://learn.microsoft.com/en-us/azure/app-service/

•	Microsoft. (2023). Introduction to Azure Blob Storage. Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blobs-introduction

•	Microsoft. (2023). Entity Framework Core documentation. Available at: https://learn.microsoft.com/en-us/ef/core/ 

•	 Connolly, T. M. & Begg, C. E. (2015). Database Systems: A Practical Approach. 6th ed.

•	Coyne, J. (2021). CSS Refactoring: Architecting Systems for Change. 2nd edition.

•	Elmasri, R. & Navathe, S. B. (2017). Fundamentals of Database Systems. 7th edition.

•	Freeman, A. (2022). Pro ASP.NET Core 6. 9th edition. Apress.

•	Lerman, J. & Miller, R. (2015). Programming Entity Framework: DbContext. 2nd edition.

•	Lucid Software Inc. (2026). Lucidchart Cloud-based Visual Workspace. [Online].

•	Microsoft. (2026). Microsoft Copilot. Available at: https://copilot.microsoft.com/ 

•	Microsoft. (2023). ASP.NET Core Middleware. [Online].

•	Microsoft. (2023). Configuration in ASP.NET Core. [Online].

•	Microsoft. (2023). Model-View-Controller (MVC) Pattern. [Online].

•	Microsoft. (2023). Primary and Foreign Key Constraints. [Online].

•	Microsoft. (2023). Azure Blob storage documentation. [Online]. Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/

•	Microsoft. (2023). Dependency injection in ASP.NET Core. [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection

•	Microsoft. (2023). Complex Querying in Entity Framework Core. [Online]. Available at: https://learn.microsoft.com/en-us/ef/core/querying/ 

•	Microsoft. (2023). Handle concurrency conflicts in EF Core. [Online]. Available at: https://learn.microsoft.com/en-us/ef/core/saving/concurrency

•	Arif, T. (2025). A comprehensive survey of privacy enhancing and trust centric cloud native security techniques against cyber threats. Journal of Cloud Computing. 12(1). Available at: https://www.mdpi.com/1424-8220/25/8/2350

•	Babucea, A. N. (2021). SQL or NoSQL databases? Critical differences. Annals of the Constantin Brâncuși University of Târgu Jiu, Economy Series. 1(1). Available at: https://ideas.repec.org/a/cbu/jrnlec/y2021v1p53-59.html

•	Borky, J. M. & Bradley, T. H. (2018). Effective Model Based Systems Engineering. Springer International Publishing. Available at: https://doi.org/10.1007/978-3-319-95669-5

•	Morrow, T. (2013). Cloud security best practices derived from mission thread analysis. Software Engineering Institute, Carnegie Mellon University. Available at: https://kilthub.cmu.edu/articles/report/Cloud_Security_Best_Practices_Derived_from_Mission_Thread_Analysis/12363563

•	Paci, H. (2022). SQL vs NoSQL databases from developer point of view. International Journal of Computer Science and Information Technology. 3(9). Available at: https://stumejournals.com/journals/i4/2022/3/95

•	Sobral, V. A. L. (2026). Experimental evaluation of serverless data layer architectures for smart city Internet of Things applications. MDPI. 9(5). Available at: https://www.mdpi.com/2624-6511/9/5/80

•	Yu, J. & Buyya, R. (2009). Gridbus workflow enactment engine. In Grid Computing. CRC Press. Available at: https://clouds.cis.unimelb.edu.au/papers/GridbusWorkflowEngine2008.pdf


