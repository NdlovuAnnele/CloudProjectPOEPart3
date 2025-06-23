using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace MVCEventEaseApp.Models
{
    public class Bookings
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public int EventID { get; set; }
        [Required]
        public int VenueID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        public virtual Venues? Venue { get; set; }
        public virtual Events? Event { get; set; }




    }
}
