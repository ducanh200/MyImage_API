namespace MyImage_API.DTOs
{
    public class OrderDTO
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public virtual UserDTO user { get; set; }
        public int feedback_id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public virtual FeedbackDTO feedback { get; set; }
        public int total_amount { get; set; }
        public int status {  get; set; }
        public DateTime created_at { get; set; }
    }
}
