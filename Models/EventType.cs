using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CLDV6211_Assignment_Part_1_St10449059.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        // Relational link to Events
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}