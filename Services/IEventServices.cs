using System;
using System.Threading.Tasks;
using WorkOutTogether.Models;

namespace WorkOutTogether.Services
{
    public interface IEventServices
    {
        Task<Event[]> GetActiveEvents(User user);
        Task<bool> AddEventAsync(Event newEvent, User user);
        Task<bool> JoinEventAsync(Event joiningEvent, User joiningUser);
        Task<Event> GetEvent(Guid IdEvent);
        Task<Event[]> GetEventCreated(String idUser);
    }
}