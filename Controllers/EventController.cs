using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkOutTogether.Data;
using WorkOutTogether.Models;
using WorkOutTogether.Services;

namespace WorkOutTogether.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventServices _eventService;
        private readonly UserManager<User> _userManager;

        public EventController(IEventServices eventService, UserManager<User> userManager)
        {
            _eventService = eventService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var events = await _eventService.GetActiveEvents(currentUser);

            var model = new EventViewModel()
            {
                Events = events
            };

            return View(model);
        }

        public IActionResult EventForm()
        {
            return View();
        }
        public async Task<IActionResult> AddEvent(Event newEvent)
        {
            //  if (!ModelState.IsValid)
            //  {
            //      return RedirectToAction("Index");
            //  }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Index");
            }

            var successful = await _eventService.AddEventAsync(newEvent, currentUser);
            if (!successful)
            {
                return BadRequest("Could not add item.");
            }

            return RedirectToAction("Index");
        }
        
        [Route("{controller}/{action}/{id?}")]
        public async Task<IActionResult> JoinEvent(Guid id)
        {
            var currenEvent = await _eventService.GetEvent(id);

            var currentUser = await _userManager.GetUserAsync(User);
            if(currenEvent.CurrentPeopleNumber < currenEvent.HowManyPeople)
            {
                var successful = await _eventService.JoinEventAsync(currenEvent, currentUser);
                if(!successful)
                {
                    currenEvent.CurrentPeopleNumber++;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DisplayCreatedEvent()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var events = await _eventService.GetEventCreated(currentUser.Id);

            var model = new EventViewModel()
            {
                Events = events
            };

            return View(model);
        }

    
    }
}