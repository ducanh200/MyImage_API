using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Feedback
{
    public class CreateFeedback
    {
        [Required]
        public int user_id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string message { get; set; }

    }
}
