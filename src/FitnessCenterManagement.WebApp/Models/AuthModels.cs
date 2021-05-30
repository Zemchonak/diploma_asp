using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Models.Resources.RegisterViewModelRes))]
        [EmailAddress(ErrorMessageResourceType = typeof(Models.Resources.RegisterViewModelRes), ErrorMessageResourceName = "EmailNotValid")]
        [Display(ResourceType = typeof(Models.Resources.RegisterViewModelRes), Name = "EmailLabel")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "SurnameRequired", ErrorMessageResourceType = typeof(Models.Resources.RegisterViewModelRes))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Models.Resources.RegisterViewModelRes), Name = "SurnameLabel")]
        public string Surname { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Models.Resources.RegisterViewModelRes))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Models.Resources.RegisterViewModelRes), Name = "FirstNameLabel")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Models.Resources.RegisterViewModelRes), Name = "LastNameLabel")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Models.Resources.RegisterViewModelRes), Name = "AddressLabel")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Models.Resources.RegisterViewModelRes))]
        [StringLength(100, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(Models.Resources.RegisterViewModelRes), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Models.Resources.RegisterViewModelRes), Name = "PasswordLabel")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Models.Resources.RegisterViewModelRes), Name = "ConfirmPasswordLabel")]
        [Compare("Password", ErrorMessageResourceType = typeof(Models.Resources.RegisterViewModelRes), ErrorMessageResourceName = "ConfirmPasswordNotMatch")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginModel
    {
        private string _email;

        private string _password;

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Models.Resources.LoginViewModelRes))]
        [EmailAddress(ErrorMessageResourceType = typeof(Models.Resources.LoginViewModelRes), ErrorMessageResourceName = "EmailNotValid")]
        [Display(ResourceType = typeof(Models.Resources.RegisterViewModelRes), Name = "EmailLabel")]
        public string Email { get => _email; set => _email = value; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Models.Resources.LoginViewModelRes))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Models.Resources.RegisterViewModelRes), Name = "PasswordLabel")]
        public string Password { get => _password; set => _password = value; }
    }
    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceName = "OldPasswordRequired", ErrorMessageResourceType = typeof(Models.Resources.ChangePasswordViewModelRes))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Models.Resources.ChangePasswordViewModelRes), Name = "OldPasswordLabel")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "NewPasswordRequired", ErrorMessageResourceType = typeof(Models.Resources.ChangePasswordViewModelRes))]
        [MinLength(6, ErrorMessageResourceName = "MinPasswordLength", ErrorMessageResourceType = typeof(Models.Resources.ChangePasswordViewModelRes))]
        [MaxLength(100, ErrorMessageResourceName = "MaxPasswordLength", ErrorMessageResourceType = typeof(Models.Resources.ChangePasswordViewModelRes))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Models.Resources.ChangePasswordViewModelRes), Name = "NewPasswordLabel")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "NotMatching", ErrorMessageResourceType = typeof(Models.Resources.ChangePasswordViewModelRes))]
        [Display(ResourceType = typeof(Models.Resources.ChangePasswordViewModelRes), Name = "ConfirmPasswordLabel")]
        public string ConfirmPassword { get; set; }
    }
}
