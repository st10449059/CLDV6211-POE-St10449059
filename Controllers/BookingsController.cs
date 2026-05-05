using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CLDV6211_Assignment_Part_1_St10449059.Data;
using CLDV6211_Assignment_Part_1_St10449059.Models;

namespace CLDV6211_Assignment_Part_1_St10449059.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(string searchString)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(s => s.Event.EventName.Contains(searchString)
                                            || s.BookingId.ToString().Equals(searchString));
            }

            return View(await bookings.ToListAsync());
        }

        // ADDED: Missing GET Create method to prevent "Page not working" error
        public IActionResult Create()
        {
            var eventList = _context.Events.ToList();
            if (!eventList.Any())
            {
                TempData["Error"] = "Please create an Event first before making a booking.";
                return RedirectToAction("Index", "Events");
            }

            ViewData["EventId"] = new SelectList(eventList, "EventId", "EventName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,AttendeeName,EventId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            return View(booking);
        }

        private bool BookingExists(int id) => _context.Bookings.Any(e => e.BookingId == id);
    }
}