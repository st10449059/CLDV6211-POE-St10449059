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