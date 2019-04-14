namespace WorkOutTogether.Models
{
    public class EventsForDetailsViewModel
    {
        public EventWithStatus EventDetailed { get; set; } 
        public User[] UsersAccepted {get; set; }
        public User[] UsersToAccept {get;set;}
        public User Owner { get; set; }

    }
}