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
            return await _context.Event
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
            var eventRequest = await _context.EventRequest.Where(x => x.EventId == joiningEvent.Id && x.UserId == joiningUser.Id).FirstOrDefaultAsync();
            if(eventRequest == null)
            {
                eventRequest = new EventRequest();
                eventRequest.RequestId = Guid.NewGuid();
                eventRequest.EventId = joiningEvent.Id;
                eventRequest.UserId = joiningUser.Id;
                eventRequest.Status = 1;

                _context.EventRequest.Add(eventRequest);

                var saveResult = await _context.SaveChangesAsync();
                return saveResult == 1;
            }
            else{
                eventRequest.Status = 1;
                _context.EventRequest.Update(eventRequest);

                var saveResult = await _context.SaveChangesAsync();
                return saveResult == 1;
            }
        }
        public async Task<bool> ResignEventAsync(Event resigningEvent, User resigningUser)
        {
            var eventRequest = await _context.EventRequest.Where(x => x.EventId == resigningEvent.Id && x.UserId == resigningUser.Id).FirstOrDefaultAsync();

            if (eventRequest == null)
            {
                return false;
            }

            eventRequest.Status = 0;

            _context.EventRequest.Update(eventRequest);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<int> GetEventStatus(Guid itemId, string userId)
        {
            var temp = await _context.EventRequest.Where(x => x.EventId == itemId && x.UserId == userId).FirstOrDefaultAsync();
            return temp.Status;
        }

        public async Task<EventWithStatus[]> GetEventWithStatus(string userId)
        {
            var events = await _context.Event.ToArrayAsync();
            var eventsWithStatus = new List<EventWithStatus>();

            foreach (var item in events)
            {
                var singleEventWithStatus = new EventWithStatus()
                {
                    Id = item.Id,
                    Name = item.Name,
                    OwnerId = item.OwnerId,
                    HowManyPeople = item.HowManyPeople,
                    CurrentPeopleNumber = item.CurrentPeopleNumber,
                    StartDate = item.StartDate,
                    longitude = item.longitude,
                    latitude = item.latitude,
                };

                var statusTemp = await _context.EventRequest.Where(x => x.EventId == item.Id && x.UserId == userId).FirstOrDefaultAsync();
                if (statusTemp == null){
                    singleEventWithStatus.Status = 0;
                }else{
                    singleEventWithStatus.Status = statusTemp.Status;
                }
                singleEventWithStatus.doIOwnedIt = (item.OwnerId == userId);

                eventsWithStatus.Add(singleEventWithStatus);
            }

            return eventsWithStatus.ToArray();
        }

        public async Task<EventWithStatus> GetSingleEventWithStatus(string userId, Guid eventId)
        {
            var currentEvent = await _context.Event.Where(x => x.Id == eventId).FirstOrDefaultAsync();
            var currentEventWithStatus = new EventWithStatus()    
                {
                    Id = currentEvent.Id,
                    Name = currentEvent.Name,
                    OwnerId = currentEvent.OwnerId,
                    HowManyPeople = currentEvent.HowManyPeople,
                    CurrentPeopleNumber = currentEvent.CurrentPeopleNumber,
                    StartDate = currentEvent.StartDate,
                    longitude = currentEvent.longitude,
                    latitude = currentEvent.latitude,
                };

                var statusTemp = await _context.EventRequest.Where(x => x.EventId == currentEvent.Id && x.UserId == userId).FirstOrDefaultAsync();
                if (statusTemp == null)
                {
                    currentEventWithStatus.Status = 0;
                }
                else
                {
                    currentEventWithStatus.Status = statusTemp.Status;
                }
                    currentEventWithStatus.doIOwnedIt = (currentEvent.OwnerId == userId);
            
                return currentEventWithStatus;
            }

        public async Task<User> GetOwnerAsync(string ownerId)
        {
            return await _context.Users.Where(x => x.Id == ownerId).FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveEvent(Guid eventId, string userId)
        {
            var eventRequest = await _context.Event.Where(x => x.Id == eventId && x.OwnerId == userId).FirstOrDefaultAsync();
            if (eventRequest == null)
            {
                return false;
            }

            _context.Event.Remove(eventRequest);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}