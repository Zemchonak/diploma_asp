using FitnessCenterManagement.WebApp.Views.Specializations.Resources;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.WebApp.Models
{
    public class SpecializationModel
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Specializations), Name = "InfoLabel")]
        public string Info { get; set; }
    }
}
