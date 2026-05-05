/* * CODE ATTRIBUTION
 * ------------------------------------------------------------------------------------------
 * Author/Source: Microsoft Documentation (ASP.NET Core Fundamentals)
 * Date: 15 April 2026
 * Description: Configuration of the web application builder, dependency injection for 
 * SQL LocalDB, and the middleware request pipeline.
 * Link: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/startup
 * ------------------------------------------------------------------------------------------
 */
using Microsoft.EntityFrameworkCore;
using CLDV6211_Assignment_Part_1_St10449059.Data;
using CLDV6211_Assignment_Part_1_St10449059.Services; // Ensure this matches your Service namespace

namespace CLDV6211_Assignment_Part_1_St10449059
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /* * 1. Database Service Registration: 
             * Registering the ApplicationDbContext into the Dependency Injection (DI) container. 
             */
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            /* * PART 2: Azure Blob Storage Service Registration
             * We retrieve the "AzureBlobStorage" connection string from appsettings.json
             * and register the BlobService as a Singleton.
             */
            var blobConnection = builder.Configuration.GetConnectionString("AzureBlobStorage");
            builder.Services.AddSingleton(x => new BlobService(blobConnection));

            // Registers the MVC services required to handle Controllers and Views.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            /* * 2. Middleware Pipeline Configuration:
             * Defines how HTTP requests are handled as they travel through the application.
             */
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

            /* * 3. Routing:
             * Defines the Conventional Routing pattern used to map URLs to actions.
             */
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
/* * =========================================================================================
 * REFERENCE LIST & ATTRIBUTION (UPDATED FOR PART 2)
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
 *   https://learn.microsoft.com/en-us/azure/storage/blobs/ [Accessed 05 May 2026].
 * =========================================================================================
 */