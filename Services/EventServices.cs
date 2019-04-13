using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkOutTogether.Data;
using WorkOutTogether.Models;

namespace WorkOutTogether.Services
{
    public class EventServices : IEventServices
    {
        private readonly ApplicationDbContext _context;
        public EventServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Event> GetEvent(Guid IdEvent)
        {
            return await _context.Event.FirstOrDefaultAsync(x => x.Id == IdEvent);
        }

        public async Task<Event[]> GetEventCreated(String idUser)
        {
            return await _context.Event
                .Where(x => x.OwnerId == idUser)
                .ToArrayAsync();
        }

        public async Task<List<Event>> GetEventJoined(String idUser)
        {
            var Events = await _context.EventRequest
                .Where(x => x.Status == 1 && x.UserId == idUser)
                .ToListAsync();

            var EventsToRetern = new List<Event>();
            foreach (var item in Events)
            { 
                var temp = await _context.Event.FirstOrDefaultAsync(x => x.Id == item.EventId);
                EventsToRetern.Add(temp);
            }

            return EventsToRetern;
        }
        public async Task<List<User>> GetUsersJoined(Guid idEvent)
        {
            var Events = await _context.EventRequest
                .Where(x=> x.Status == 1 && x.EventId == idEvent)
                .ToListAsync();
            
            var UsersToRetern = new List<User>();
            foreach (var item in Events)
            { 
                var temp = await _context.Users.FirstOrDefaultAsync(x => x.Id == item.UserId);
                UsersToRetern.Add(temp);
            }

            return UsersToRetern;
        }
        public async Task<Event[]> GetActiveEvents(User user)
        {
            return await _context.Event.Where(x => x.OwnerId != user.Id)
                .ToArrayAsync();
        }

        public async Task<bool> AddEventAsync(Event newEvent, User user)
        {
            newEvent.Id = Guid.NewGuid();
            newEvent.OwnerId = user.Id;
            newEvent.StartDate = DateTime.Now;
            newEvent.CurrentPeopleNumber = 0;
            
            _context.Event.Add(newEvent);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
            
        }

        public async Task<bool> JoinEventAsync(Event joiningEvent, User joiningUser)
        {
            var eventRequest = new EventRequest();
            eventRequest.RequestId = Guid.NewGuid();
            eventRequest.EventId = joiningEvent.Id;
            eventRequest.UserId = joiningUser.Id;
            eventRequest.Status = 1;

            _context.EventRequest.Add(eventRequest);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}