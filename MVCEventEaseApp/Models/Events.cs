using System.ComponentModel.DataAnnotations;

namespace MVCEventEaseApp.Models
{
    public class Events
    {
        [Key]
        public int EventID { get; set; }
        [Required]
        public string EventName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Date of the Event")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter Description")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "The Description must be between 3 and 100 characters!")]
        public string Description { get; set; } = string.Empty;

     
        public int VenueID { get; set; }

        public virtual Venues? Venue { get; set; }

        public ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();

        public int? EventtypeID { get; set; }
        public EventType? EventType { get; set; }


    }
}
