﻿using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Frame
{
    public class EditFrame
    {
        [Required]
        public int id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá tiền !")]
        public int frame_amount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục !")]
        public string frame_name { get; set; }

        [Required]
        public string frame_color_outsite { get; set; }

        [Required]
        public string frame_color_insite { get; set; }

    }
}
