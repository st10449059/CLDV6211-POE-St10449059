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
 * REFERENCE LIST & ATTRIBUTION (FINAL)
 * =========================================================================================
 * · Connolly, T. M. & Begg, C. E. (2015). Database Systems: A Practical Approach. 6th ed.
 * · Coyne, J. (2021). CSS Refactoring: Architecting Systems for Change. 2nd edition.
 * · Elmasri, R. & Navathe, S. B. (2017). Fundamentals of Database Systems. 7th edition.
 * · Freeman, A. (2022). Pro ASP.NET Core 6. 9th edition. Apress.
 * · Lerman, J. & Miller, R. (2015). Programming Entity Framework: DbContext. 2nd edition.
 * · Lucid Software Inc. (2026). Lucidchart Cloud-based Visual Workspace. [Online].
 * · Microsoft. (2023). ASP.NET Core Middleware. [Online].
 * · Microsoft. (2023). Configuration in ASP.NET Core. [Online].
 * · Microsoft. (2023). Model-View-Controller (MVC) Pattern. [Online].
 * · Microsoft. (2023). Primary and Foreign Key Constraints. [Online].
 * · Microsoft. (2023). Azure Blob storage documentation. [Online]. Available at: 
 * https://learn.microsoft.com/en-us/azure/storage/blobs/ [Accessed 02 June 2026].
 * · Microsoft. (2023). Dependency injection in ASP.NET Core. [Online]. Available at:
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
 * · Microsoft. (2023). Complex Querying in Entity Framework Core. [Online]. Available at: 
 * https://learn.microsoft.com/en-us/ef/core/querying/ [Accessed 02 June 2026].
 * · Microsoft. (2023). Handle concurrency conflicts in EF Core. [Online]. Available at: 
 * https://learn.microsoft.com/en-us/ef/core/saving/concurrency [Accessed 02 June 2026].
 * =========================================================================================
 */