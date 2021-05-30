using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FitnessCenterManagement.WebApp.Views.Profile.Resources;

namespace FitnessCenterManagement.WebApp.Models
{
    public class ProfileModel
    {
        public string Surname { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public decimal Balance { get; set; }

        public string Language { get; set; }

        public string LanguageName { get; set; }

        public string RoleName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public IReadOnlyCollection<LanguageModel> Languages { get; set; }
    }

    public class ProfileEditModel
    {
        [Required(ErrorMessageResourceName = "SurnameRequired", ErrorMessageResourceType = typeof(Models.Resources.EditProfileViewModelRes))]
        [DataType(DataType.Text)]
        [MaxLength(256, ErrorMessageResourceName = "MaxSurnameLength", ErrorMessageResourceType = typeof(Models.Resources.EditProfileViewModelRes))]
        [Display(ResourceType = typeof(ProfileRes), Name = "SurnameLabel")]
        public string Surname { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Models.Resources.EditProfileViewModelRes))]
        [DataType(DataType.Text)]
        [MaxLength(256, ErrorMessageResourceName = "MaxFirstNameLength", ErrorMessageResourceType = typeof(Models.Resources.EditProfileViewModelRes))]
        [Display(ResourceType = typeof(ProfileRes), Name = "FirstNameLabel")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(256, ErrorMessageResourceName = "MaxLastNameLength", ErrorMessageResourceType = typeof(Models.Resources.EditProfileViewModelRes))]
        [Display(ResourceType = typeof(ProfileRes), Name = "LastNameLabel")]
        public string LastName { get; set; }
        
        [DataType(DataType.Text)]
        [MaxLength(256, ErrorMessageResourceName = "MaxAddressLength", ErrorMessageResourceType = typeof(Models.Resources.EditProfileViewModelRes))]
        [Display(ResourceType = typeof(ProfileRes), Name = "AddressLabel")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Models.Resources.EditProfileViewModelRes))]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(ProfileRes), Name = "EmailLabel")]
        public string Email { get; set; }
    }

    public class ProfileChangeBalanceModel
    {
        [Required]
        [DataType(DataType.Currency)]
        [Display(ResourceType = typeof(ProfileRes), Name = "BalanceLabel")]
        public decimal Balance { get; set; }
    }
}
