using System;
using System.Collections.Generic;
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

            var events = await _eventService.GetEventWithStatus(currentUser.Id);

            
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

        [Route("{controller}/{action}/{id?}")]
        public async Task<IActionResult> ResignEvent(Guid id)
        {
            var currenEvent = await _eventService.GetEvent(id);

            var currentUser = await _userManager.GetUserAsync(User);
            if (currenEvent.CurrentPeopleNumber < currenEvent.HowManyPeople)
            {
                var successful = await _eventService.ResignEventAsync(currenEvent, currentUser);
                if (!successful)
                {
                    currenEvent.CurrentPeopleNumber--;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DisplayEvents()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var eventsCreated = await _eventService.GetEventCreated(currentUser.Id);
            var eventsJoined = await _eventService.GetEventJoined(currentUser.Id);

            var model = new EventToDisplayViewModel()
            {
                EventsCreated = eventsCreated,
                EventsJoined = eventsJoined.ToArray() 
            };

            return View(model);
        }

        [Route("{controller}/{action}/{id?}")]
        public async Task<IActionResult> EventDetails(Guid id)
        {
            if(id == null)
                return RedirectToAction("Index");
                
            var currentUser = await _userManager.GetUserAsync(User);
            var currenEvent = await _eventService.GetSingleEventWithStatus(currentUser.Id, id);
            var userJoined = await _eventService.GetUsersJoined(id);
            var owner = await _eventService.GetOwnerAsync(currenEvent.OwnerId);

            var model = new EventsForDetailsViewModel()
            {
                EventDetailed = currenEvent,
                Users = userJoined.ToArray(),
                Owner = owner
            };

            return View(model);
        }

        [Route("{controller}/{action}/{id?}")]
        public async Task<IActionResult> RemoveEvent(Guid eventId)
        {
            if (eventId == null)
                return RedirectToAction("Index");

            Console.WriteLine("aer" + eventId);
            var currentUser = await _userManager.GetUserAsync(User);
            var succeed = await _eventService.RemoveEvent(eventId, currentUser.Id);

            if(succeed)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}