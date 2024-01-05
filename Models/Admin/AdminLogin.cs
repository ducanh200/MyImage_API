using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Admin
{
    public class AdminLogin
    {
        [Required]
        public string email { get; set; }

        [Required]
        [MinLength(6)]
        public string password { get; set; }
    }
}
