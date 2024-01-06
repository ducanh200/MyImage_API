using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Size
{
    public class EditSize
    {
        [Required]
        public int id { get; set; }

        [Required]          
        public int size_amount { get; set; }

        [Required]
        public string size_name { get; set; }

        [Required]
        public string size_width { get; set; }

    }
}
