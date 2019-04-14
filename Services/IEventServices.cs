using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkOutTogether.Models;

namespace WorkOutTogether.Services
{
    public interface IEventServices
    {
        Task<Event[]> GetActiveEvents(User user);
        Task<bool> AddEventAsync(Event newEvent, User user);
        Task<bool> JoinEventAsync(Event joiningEvent, User joiningUser);
        Task<bool> ResignEventAsync(Event resigningEvent,User resigningUser);
        Task<Event> GetEvent(Guid IdEvent);
        Task<Event[]> GetEventCreated(String idUser);
        Task<List<Event>> GetEventJoined(String idUser);
        Task<int> GetEventStatus(Guid itemId, string userId);
        Task<EventWithStatus[]> GetEventWithStatus(string userId);
        Task<List<User>> GetUsersJoined(Guid idEvent);
        Task<User> GetOwnerAsync(string ownerId);
        Task<EventWithStatus> GetSingleEventWithStatus(string userId, Guid eventId);
        Task<bool> RemoveEvent(Guid eventId, string userId);
    }
}