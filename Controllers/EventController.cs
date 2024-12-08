using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDBcontext _context;
        private readonly UserManager<User> _userManager;

        public EventController(AppDBcontext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Display events of the logged-in user
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var userEvents = _context.Events.Where(e => e.UserId == userId).ToList();
            return View(userEvents);
        }

        // Show add event form
        public IActionResult AddEvent()
        {
            return View();
        }

        // Action to display all events
        public async Task<IActionResult> EventTab()
        {
            // Get all events from the database
            var events = await _context.Events.ToListAsync();

            // Pass the events to the view
            return View(events);
        }


        // Handle add event form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(Event eventModel)
        {
            if (ModelState.IsValid)
            {
                // Get the user ID from the logged-in user
                var userId = _userManager.GetUserId(User);

                // Set the UserId for the event
                eventModel.UserId = userId;

                // Save the event
                _context.Add(eventModel);
                await _context.SaveChangesAsync();

                
                return RedirectToAction("Index");  // Redirect to event list or another page
            }

            // If the model state is invalid, redisplay the form with validation errors
            return View(eventModel);
        }



        // Show edit event form
        public async Task<IActionResult> EditEvent(int id)
        {
            var eventToEdit = await _context.Events.FindAsync(id);
            if (eventToEdit == null || eventToEdit.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            return View(eventToEdit);
        }

        // Handle edit event form submission
        [HttpPost]
        public async Task<IActionResult> EditEvent(Event eventModel)
        {
            if (!ModelState.IsValid)
            {
                return View(eventModel);
            }

            var eventToUpdate = await _context.Events.FindAsync(eventModel.Id);
            if (eventToUpdate == null || eventToUpdate.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            eventToUpdate.Name = eventModel.Name;
            eventToUpdate.EventDate = eventModel.EventDate;
            eventToUpdate.Location = eventModel.Location;
            eventToUpdate.Description = eventModel.Description;
            eventToUpdate.SeatsAvailable = eventModel.SeatsAvailable;

            _context.Events.Update(eventToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Delete event
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete == null || eventToDelete.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Register(int eventId)
        {
            // Get the current logged-in user's ID
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if user is not logged in
            }

            // Find the event the user wants to register for
            var eventToRegister = await _context.Events.FindAsync(eventId);
            if (eventToRegister == null)
            {
                return NotFound(); // Event not found
            }

            if (eventToRegister.SeatsAvailable <= 0)
            {
                // If no seats are available, return an error
                TempData["Error"] = "No seats available for this event.";
                return RedirectToAction("EventTab");
            }

            // Register the user by decreasing the available seats
            eventToRegister.SeatsAvailable -= 1;
            _context.Update(eventToRegister);
            await _context.SaveChangesAsync();

            // Optionally, save the registration to another table for user tracking
            // For example: _context.EventRegistrations.Add(new EventRegistration { UserId = userId, EventId = eventId });
            // await _context.SaveChangesAsync();

            TempData["Success"] = "You have successfully registered for the event!";
            return RedirectToAction("EventTab"); // Redirect back to the events page
        }

    }
}
