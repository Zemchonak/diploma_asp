using FitnessCenterManagement.WebApp.Attributes;
using FitnessCenterManagement.WebApp.Views.Abonements.Resources;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.WebApp.Models
{
    public class AbonementModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Models.Resources.AbonementsRes))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(AbonementsRes), Name = "NameLabel")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "CoefficientRequired", ErrorMessageResourceType = typeof(Models.Resources.AbonementsRes))]
        [MinCoefficientValue(ErrorMessageResourceName = "MinCoefficientValue", ErrorMessageResourceType = typeof(Models.Resources.AbonementsRes))]
        [MaxCoefficientValue(ErrorMessageResourceName = "MaxCoefficientValue", ErrorMessageResourceType = typeof(Models.Resources.AbonementsRes))]
        [DataType(DataType.Currency)]
        [Display(ResourceType = typeof(AbonementsRes), Name = "CoefficientLabel")]
        public decimal? Coefficient { get; set; }

        [Required(ErrorMessageResourceName = "AttendancesRequired", ErrorMessageResourceType = typeof(Models.Resources.AbonementsRes))]
        [MinAttendencesValue(ErrorMessageResourceName = "MinAttendancesValue", ErrorMessageResourceType = typeof(Models.Resources.AbonementsRes))]
        [Display(ResourceType = typeof(AbonementsRes), Name = "AttendancesLabel")]
        public int? Attendances { get; set; }

        [Display(ResourceType = typeof(AbonementsRes), Name = "StatusLabel")]
        public AbonementStatus Status { get; set; }

        public string ImageName { get; set; }
    }
}
