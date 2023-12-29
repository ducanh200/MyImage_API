using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Size
{
    public class CreateSize
    {
        [Required]
        public int size_amount { get; set; }

        [Required]
        public string size_name { get; set; }

        [Required]
        public string size_width { get; set;}

        [Required]
        public string size_height { get; set;}
    }
}
