using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace MVCEventEaseApp.Models
{
    public class EventType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<Events>? Events { get; set; }

    }
}
