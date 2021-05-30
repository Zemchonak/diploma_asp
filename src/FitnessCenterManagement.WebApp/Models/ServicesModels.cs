using FitnessCenterManagement.WebApp.Views.Services.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string SpecializationInfo { get; set; }

        public int SpecializationId { get; set; }

        public string Description { get; set; }
    }

    public class ServiceEditingModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Models.Resources.ServicesRes))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Views.Services.Resources.ServicesRes), Name = "NameLabel")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "PriceRequired", ErrorMessageResourceType = typeof(Models.Resources.ServicesRes))]
        [DataType(DataType.Currency)]
        [Display(ResourceType = typeof(Views.Services.Resources.ServicesRes), Name = "PriceLabel")]
        public decimal Price { get; set; }

        public string SpecializationInfo { get; set; }

        [Display(ResourceType = typeof(Views.Services.Resources.ServicesRes), Name = "SpecializationLabel")]
        public int SpecializationId { get; set; }

        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Views.Services.Resources.ServicesRes), Name = "DescriptionLabel")]
        public string Description { get; set; }
    }
}
