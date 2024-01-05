using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Hanger
{
    public class EditHanger
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int hanger_amount { get; set; }
        [Required]
        public string hanger_name { get; set; }
    }
}
