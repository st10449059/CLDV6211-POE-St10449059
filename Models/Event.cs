using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLDV6211_Assignment_Part_1_St10449059.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [Display(Name = "Date and Time")]
        public DateTime EventDate { get; set; }

        public string? ImageUrl { get; set; }

        // --- NEW: Event Type Lookup Relationship ---
        [Required]
        [Display(Name = "Event Category")]
        public int EventTypeId { get; set; }

        [ForeignKey("EventTypeId")]
        public virtual EventType? EventType { get; set; }
        // -------------------------------------------

        public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        public virtual Venue? Venue { get; set; }

        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}
