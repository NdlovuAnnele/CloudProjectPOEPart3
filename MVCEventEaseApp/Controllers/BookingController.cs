using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCEventEaseApp.Data;
using MVCEventEaseApp.Models;

namespace MVCEventEaseApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BookingController(ApplicationDbContext db)
        {
            _db = db;
        }
#nullable disable
        public IActionResult Index(string searchString, int? EventTypeID, DateTime? startDate, DateTime? endDate, bool? availability)
        {
            var bookings = _db.Bookings
         .Include(b => b.Event)
         .ThenInclude(e => e.Venue)
         .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b =>
                    b.BookingID.ToString().Contains(searchString) ||
                    b.Event.EventName.Contains(searchString));
            }
            if (EventTypeID.HasValue)
            {
                bookings = bookings.Where(b => b.Event.EventtypeID == EventTypeID);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                bookings = bookings.Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate);
            }

            if (availability.HasValue)
            {
                bookings = bookings.Where(b => b.Event.Venue.IsAvailable == availability.Value);
            }

            ViewBag.EventTypes = new SelectList(_db.EventTypes.ToList(), "Id", "Name");

            return View(bookings.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var venues = _db.Venues.ToList();
            ViewBag.VenueList = new SelectList(venues, "VenueID", "VenueName");

            var events = _db.Events.ToList();
            ViewBag.EventList = new SelectList(events, "EventID", "EventName");


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Bookings obj)
        {
            try
            {
                // Check for double booking
                if (obj.VenueID != 0 && obj.BookingDate != DateTime.MinValue)
                {
                    bool isBooked = _db.Bookings.Any(b =>
                        b.VenueID == obj.VenueID &&
                        b.BookingDate.Date == obj.BookingDate.Date);

                    if (isBooked)
                    {
                        ModelState.AddModelError("", "This venue is already booked on the selected date.");
                    }
                }

                // ✅ Now always check ModelState after all potential errors added
                if (ModelState.IsValid)
                {
                    _db.Bookings.Add(obj);
                    _db.SaveChanges();
                    TempData["Success"] = "Booking successfully created.";
                    return RedirectToAction("Index", "Booking");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
            }

            // Ensure dropdowns are populated even if there's an error
            ViewBag.VenueList = new SelectList(_db.Venues, "VenueID", "VenueName", obj?.VenueID);
            ViewBag.EventList = new SelectList(_db.Events, "EventID", "EventName", obj?.EventID);

            return View(obj);

        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var venues = _db.Venues.ToList();
            ViewBag.VenueList = new SelectList(venues, "VenueID", "VenueName");
            var events = _db.Events.ToList();
            ViewBag.EventList = new SelectList(events, "EventID", "EventName");

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var bookingFromDb = _db.Bookings.FirstOrDefault(b => b.BookingID == id);
            if (bookingFromDb == null)
            {
                return NotFound();
            }
            return View(bookingFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Bookings obj)
        {
            if (obj == null)
            {
                return BadRequest(); // or return a custom error view
            }

            // Prevent double booking of the same venue on the same date (excluding current booking)
            bool isAlreadyBooked = _db.Bookings.Any(b =>
                b.VenueID == obj.VenueID &&
                b.BookingDate.Date == obj.BookingDate.Date &&
                b.BookingID != obj.BookingID);

            if (isAlreadyBooked)
            {
                ModelState.AddModelError("", "This venue is already booked on the selected date.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Bookings.Update(obj);
                    _db.SaveChanges();
                    TempData["Success"] = "Booking updated successfully.";
                    return RedirectToAction("Index","Booking");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Something went wrong. Please try again.");
                }
            }

            // Re-populate dropdowns in case of validation errors
            ViewBag.VenueList = new SelectList(_db.Venues, "VenueID", "VenueName", obj.VenueID);
            ViewBag.EventList = new SelectList(_db.Events, "EventID", "EventName", obj.EventID);

            return View(obj);
        }
        
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var venues = _db.Venues.ToList();
            ViewBag.VenueList = new SelectList(venues, "VenueID", "VenueName");

            var events = _db.Events.ToList();
            ViewBag.EventList = new SelectList(events, "EventID", "EventName");
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var booking = _db.Bookings
                .Include(b => b.Event)
                .ThenInclude(e => e.Venue)
                .FirstOrDefault(b => b.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var booking = _db.Bookings.FirstOrDefault(b => b.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            _db.Bookings.Remove(booking);
            _db.SaveChanges();

            TempData["Success"] = "Booking deleted successfully.";
            return RedirectToAction("Index", "Booking");


        }
    }
}
