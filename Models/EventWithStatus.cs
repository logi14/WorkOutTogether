namespace WorkOutTogether.Models
{
    public class EventWithStatus : Event
    {
        public int Status { get; set; }
        public bool doIOwnedIt { get; set; }
    }
}