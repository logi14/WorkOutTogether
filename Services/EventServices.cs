using System;
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
            
            _context.Event.Add(newEvent);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
            
        }
    }
}