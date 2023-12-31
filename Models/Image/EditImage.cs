using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Image
{
    public class EditImage
    {
        [Required]
        public int id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập dữ liệu bắt buộc !")]
        public int frame_id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập dữ liệu bắt buộc !")]
        public int material_id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập dữ liệu bắt buộc !")]
        public int size_id { get; set; }

        [Required(ErrorMessage = "vui lòng nhập file ảnh")]
        public IFormFile thumbnail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng ảnh !")]
        public int quantity { get; set; }
    }
}
