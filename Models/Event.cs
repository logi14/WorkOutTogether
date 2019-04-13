using System;
using System.ComponentModel.DataAnnotations;

namespace WorkOutTogether.Models
{
    public class Event
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public string longitude { get; set; }
        [Required]
        public string latitude { get; set; }
        [Required]
        public int HowManyPeople { get; set; }
        [Required]
        public int CurrentPeopleNumber { get; set; }
    }
}