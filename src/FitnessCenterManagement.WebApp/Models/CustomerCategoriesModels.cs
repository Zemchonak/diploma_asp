using FitnessCenterManagement.WebApp.Views.CustomerCategories.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.Models
{
    public class CustomerCategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal SaleCoefficient { get; set; }
    }

    public class CustomerCategoriesEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "NameRequired",
            ErrorMessageResourceType = typeof(Models.Resources.CustomerCategoryRes))]
        [Display(ResourceType = typeof(CustomerCategoriesRes), Name = "NameLabel")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "SaleCoefficientRequired",
            ErrorMessageResourceType = typeof(Models.Resources.CustomerCategoryRes))]
        [DataType(DataType.Currency)]
        [Display(ResourceType = typeof(CustomerCategoriesRes), Name = "SaleCoefficientLabel")]
        public decimal SaleCoefficient { get; set; }
    }
}
