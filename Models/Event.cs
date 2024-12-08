using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Areas.Identity.Data;

namespace WebApplication1.Models
{
    public class Event
    {
        public int Id { get; set; } // Primary key

        [Required]
        [Display(Name = "Event Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Seats Available must be greater than zero.")]
        [Display(Name = "Seats Available")]
        public int SeatsAvailable { get; set; }

        public string UserId { get; set; } // Foreign key for the user who created the event
        
    }
}
