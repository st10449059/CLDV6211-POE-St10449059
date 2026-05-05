using CLDV6211_Assignment_Part_1_St10449059.Data;
using CLDV6211_Assignment_Part_1_St10449059.Models;
using CLDV6211_Assignment_Part_1_St10449059.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.Include(e => e.Venue).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDate,VenueId")] Event @event, IFormFile imageFile)
        {
            // Part 2 Validation: Prevent double booking of venue for same date/time
            var isAlreadyBooked = await _context.Events.AnyAsync(e => e.VenueId == @event.VenueId && e.EventDate == @event.EventDate);

            if (isAlreadyBooked)
            {
                ModelState.AddModelError("", "Validation Error: This venue is already booked for the selected date and time.");
                ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
                return View(@event);
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    @event.ImageUrl = await _blobService.UploadFileAsync(imageFile, "event-images");
                }

                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            return View(@event);
        }

        // Rest of standard methods follow...
        private bool EventExists(int id) => _context.Events.Any(e => e.EventId == id);
    }
}