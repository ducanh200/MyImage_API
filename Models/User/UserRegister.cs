using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.User
{
    public class UserRegister
    {
        
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        [MinLength(6)]
        public string password { get; set; }

    }
}
