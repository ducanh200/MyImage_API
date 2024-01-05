namespace MyImage_API.DTOs
{
    public class FeedbackDTO
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public virtual UserDTO user { get; set; }
        public string message { get; set; }
        public int rate { get; set; }
        public DateTime created_at { get; set; }
    }
}
