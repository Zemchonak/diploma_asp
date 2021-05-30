using Microsoft.AspNetCore.Http;

namespace FitnessCenterManagement.WebApp.Models
{
    public enum AbonementStatus
    {
        Disabled = 0,
        Enabled,
    }

    public enum AbonementCardStatus
    {
        Disabled = 0,
        Enabled,
    }

    public enum DateEventStatus
    {
        Cancelled = 0,
        Available,
        Started,
        Finished,
    }
    public class UserModel
    {
        public string Id { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }

    public class ErrorViewModel
    {
        public string ErrorMessage { get; set; }

        public string ErrorAttribute { get; set; }
    }

    public class LanguageModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }

    public class ChangeLanguageModel
    {
        public string LanguageCode { get; set; }
    }

    public class ImageUploadModel
    {
        public int Id { get; set; }

        public IFormFile File { get; set; }
    }
}
