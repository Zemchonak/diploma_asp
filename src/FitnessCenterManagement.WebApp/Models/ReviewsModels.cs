using FitnessCenterManagement.WebApp.Models.Resources;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.WebApp.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }

        public string UserData { get; set; }

        public string UserId { get; set; }

        public string Text { get; set; }

        public bool IsAnonymous { get; set; }

        public bool IsHidden { get; set; }
    }

    public class ReviewEditModel
    {
        public int Id { get; set; }

        public string UserData { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessageResourceName = "TextRequired", ErrorMessageResourceType = typeof(Models.Resources.ReviewsRes))]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Views.Reviews.Resources.ReviewsRes), Name = "TextLabel")]

        public string Text { get; set; }

        [Display(ResourceType = typeof(Views.Reviews.Resources.ReviewsRes), Name = "IsAnonymousLabel")]
        public bool IsAnonymous { get; set; }

        [Display(ResourceType = typeof(Views.Reviews.Resources.ReviewsRes), Name = "IsHiddenLabel")]
        public bool IsHidden { get; set; }
    }
}
