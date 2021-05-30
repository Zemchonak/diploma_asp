using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.WebApp.Models
{
    public class VenueModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Models.Resources.VenuesRes))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Views.Venues.Resources.VenueRes), Name = "NameLabel")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "LocationRequired", ErrorMessageResourceType = typeof(Models.Resources.VenuesRes))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Views.Venues.Resources.VenueRes), Name = "LocationLabel")]
        public string Location { get; set; }

        public string ImageName { get; set; }

        public string QrCodeId { get; set; }
    }
}
