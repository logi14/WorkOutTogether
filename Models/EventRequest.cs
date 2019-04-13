using System;
using System.ComponentModel.DataAnnotations;

namespace WorkOutTogether.Models
{
    public class EventRequest
    {
        [Required]
        [Key]
        public Guid RequestId { get; set; }
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int Status { get; set; }
    }
}