using System.Threading.Tasks;
using WorkOutTogether.Models;

namespace WorkOutTogether.Services
{
    public interface IEventServices
    {
        Task<Event[]> GetActiveEvents(User user);

        Task<bool> AddEventAsync(Event newEvent, User user);
    }
}