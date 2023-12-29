using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Material
{
    public class CreateMaterial
    {
        [Required]
        public int material_amount { get; set; }
        [Required]
        public string material_name { get; set; }
    }
}
