using FitnessCenterManagement.WebApp.Views.FitnessEvents.Resources;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.WebApp.Models
{
    public class FitnessEventModel
    {
        public int Id { get; set; }

        public string ServiceInfo { get; set; }

        [Display(ResourceType = typeof(FitnessEventsRes), Name = "ServiceInfoLabel")]
        [Required(ErrorMessageResourceName = "ServiceRequired", ErrorMessageResourceType = typeof(Resources.FitnessEventsRes))]
        public int ServiceId { get; set; }

        public string VenueInfo { get; set; }

        [Display(ResourceType = typeof(FitnessEventsRes), Name = "VenueInfoLabel")]
        [Required(ErrorMessageResourceName = "VenueRequired", ErrorMessageResourceType = typeof(Resources.FitnessEventsRes))]
        public int VenueId { get; set; }

        [Display(ResourceType = typeof(FitnessEventsRes), Name = "MinutesLabel")]
        [Required(ErrorMessageResourceName = "MinutesRequired", ErrorMessageResourceType = typeof(Resources.FitnessEventsRes))]
        public int Minutes { get; set; }
    }
}
