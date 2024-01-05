using System.ComponentModel.DataAnnotations;

namespace MyImage_API.Models.Order
{
    public class CreateOrder
    {
        [Required]
        public int user_id { get; set; }
        [Required]
        public string phone { get; set; }

        [Required]
        public string address { get; set; }

        [Required] 
        public string city { get; set; }


        [Required]
        public int total_amount { get; set;}

    }
}
