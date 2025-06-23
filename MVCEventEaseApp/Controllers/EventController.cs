using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCEventEaseApp.Data;
using MVCEventEaseApp.Models;

namespace MVCEventEaseApp.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EventController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var eventsWithVenue = _db.Events.Include(e => e.Venue).ToList();
            return View(eventsWithVenue);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var venues = _db.Venues.ToList();
            ViewBag.VenueList = new SelectList(venues, "VenueID", "VenueName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Events obj)
        {
            try
            {
                // Validate that the venue is not double-booked
                bool isBooked = _db.Events.Any(e =>
                    e.VenueID == obj.VenueID &&
                    e.EventDate.Date == obj.EventDate.Date);

                if (isBooked)
                {
                    ModelState.AddModelError("", "This venue is already booked on the selected date.");
                }

                // Only save if no validation errors
                if (ModelState.IsValid)
                {
                    _db.Events.Add(obj);
                    _db.SaveChanges();
                    TempData["Success"] = "Event successfully created.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
            }

            // Re-populate dropdowns in case of validation errors
            ViewBag.VenueList = new SelectList(_db.Venues, "VenueID", "VenueName", obj.VenueID);
            ViewBag.EventTypeList = new SelectList(_db.EventTypes, "EventTypeID", "Name", obj.EventtypeID);

            return View(obj);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var venues = _db.Venues.ToList();
            ViewBag.VenueList = new SelectList(venues, "VenueID", "VenueName");
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var eventFromDb = _db.Events.FirstOrDefault(e => e.EventID == id);

            if (eventFromDb == null)
            {
                return NotFound();
            }

            return View(eventFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Events obj)
        {
            if (obj == null)
            {
                return BadRequest(); // or a custom error view
            }

            if (obj.EventName.Length < 3)
            {
                ModelState.AddModelError("EventName", "Event name must be at least 3 characters long.");
            }

            // Prevent double booking (exclude current event)
            bool isBooked = _db.Events.Any(e =>
                e.VenueID == obj.VenueID &&
                e.EventDate.Date == obj.EventDate.Date &&
                e.EventID != obj.EventID);

            if (isBooked)
            {
                ModelState.AddModelError("", "Another event is already scheduled at this venue on that date.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Events.Update(obj);
                    _db.SaveChanges();
                    TempData["Success"] = "Event updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Something went wrong. Please try again.");
                }
            }

            // Re-populate dropdowns in case of error
            ViewBag.VenueList = new SelectList(_db.Venues, "VenueID", "VenueName", obj.VenueID);
            ViewBag.EventTypeList = new SelectList(_db.EventTypes, "EventTypeID", "Name", obj.EventtypeID);

            return View(obj);
        }
            [HttpGet]
        public IActionResult Delete(int? id)
        {
            var venues = _db.Venues.ToList();
            ViewBag.VenueList = new SelectList(venues, "VenueID", "VenueName");
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var eventFromDb = _db.Events.FirstOrDefault(e => e.EventID == id);
            if (eventFromDb == null)
            {
                return NotFound();
            }
            return View(eventFromDb);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var eventItem = _db.Events
                .Include(e => e.Bookings)
                .FirstOrDefault(e => e.EventID == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            if (eventItem.Bookings.Any())
            {
                TempData["Error"] = "Cannot delete event. It has active bookings.";
                return RedirectToAction("Index");
            }

            _db.Events.Remove(eventItem);
            _db.SaveChanges();
            TempData["Success"] = "Event deleted successfully.";
            return RedirectToAction("Index");

        }
    }
}
