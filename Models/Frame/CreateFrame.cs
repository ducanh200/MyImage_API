using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Frame
{
    public class CreateFrame
    {
        [Required(ErrorMessage = "Vui lòng nhập giá tiền !")]
        public int frame_amount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập màu khung !")]
        public string frame_color { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên danh mục !")]
        public string frame_name {  get; set; }

    }
}
