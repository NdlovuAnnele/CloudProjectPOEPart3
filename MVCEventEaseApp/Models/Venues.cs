using System.ComponentModel.DataAnnotations;

namespace MVCEventEaseApp.Models
{
    public class Venues
    {
        [Key]
        public int VenueID { get; set; }
        [Required(ErrorMessage ="Please enter a Venue name")]
        [StringLength(100, MinimumLength=3,ErrorMessage = "Venue name must be between 3 and 100 characters!")]
        public string VenueName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a Location")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 100 characters!")]
        public string Locations { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Capacity limit")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Capacity must be between 3 and 100 characters!")]
        [Range(1, 100000, ErrorMessage = "Capacity must be a positive number.")]
        public string Capacity { get; set; } = string.Empty;


        [Required(ErrorMessage = "Please enter ImageURL")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "ImageURL must be between 3 and 100 characters!")]
        public string ImageURL { get; set; } = string.Empty;

        public ICollection<Events> Events { get; set; } = new List<Events>();

        public bool IsAvailable { get; set; }


    }
}
