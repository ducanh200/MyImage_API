using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Admin
{
    public class AdminRegister
    {
        [Required]
        public string name { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        [MinLength(6)]
        public string password { get; set; }
    }
}
