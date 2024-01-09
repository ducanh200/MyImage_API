using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.User
{
    public class ChangePassword
    {
        [Required]
        public int id { get; set; }

        [Required]
        public string old_password { get; set; }

        [Required]
        public string new_password { get; set;}
    }
}
