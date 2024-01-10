using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Url
{
    public class CreateUrl
    {
        [Required(ErrorMessage = "vui lòng nhập file ảnh")]
        public IFormFile thumbnail { get; set; }
    }
}
