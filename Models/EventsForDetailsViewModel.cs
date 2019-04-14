namespace WorkOutTogether.Models
{
    public class EventsForDetailsViewModel
    {
        public EventWithStatus EventDetailed { get; set; } 
        public User[] Users {get; set; }
        public User Owner { get; set; }

    }
}