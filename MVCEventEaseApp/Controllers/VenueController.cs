using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCEventEaseApp.Data;
using MVCEventEaseApp.Models;

namespace MVCEventEaseApp.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VenueController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Venues> venuesList = _db.Venues.ToList();
            return View(venuesList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Venues obj)
        {
            if (obj == null)
            {
                return BadRequest(); // or return View(); with an error message
            }
            if (obj.VenueName.Length < 3)
            {
                ModelState.AddModelError("VenueName", "Venue name must be at least 3 characters long.");
            }
            if (ModelState.IsValid)
            {
                _db.Venues.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
            }
            return View(obj);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Venues? venue = _db.Venues.Find(id);


            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        [HttpPost]
        public IActionResult Edit(Venues obj)
        {
            if (obj == null)
            {
                return BadRequest(); // or return View(); with an error message
            }
            if (obj.VenueName.Length < 3)
            {
                ModelState.AddModelError("VenueName", "Venue name must be at least 3 characters long.");
            }
            if (ModelState.IsValid)
            {

                _db.Venues.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
            }
            return View(obj);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Venues? venue = _db.Venues.Find(id);


            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var venue = _db.Venues.Include(v => v.Events).ThenInclude(e => e.Bookings).FirstOrDefault(v => v.VenueID == id);

            if (venue == null) return NotFound();

            bool hasBookings = venue.Events.Any(e => e.Bookings.Any());
            if (hasBookings)
            {
                TempData["Error"] = "Cannot delete venue with active bookings.";
                return RedirectToAction("Index");
            }
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
            }
            _db.Venues.Remove(venue);
            _db.SaveChanges();
            TempData["Success"] = "Venue deleted successfully.";
            return RedirectToAction("Index");


        }
    }

    }
