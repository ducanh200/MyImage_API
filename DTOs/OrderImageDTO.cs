namespace MyImage_API.DTOs
{
    public class OrderImageDTO
    {
        public int id { get; set; }
        public int frame_id { get; set; }
        public virtual FrameDTO frame { get; set; }
        public int hanger_id { get; set; }
        public virtual HangerDTO hanger { get; set; }
        public int size_id { get; set; }
        public virtual SizeDTO size { get; set; }
        public int order_id { get; set; }
        public virtual OrderDTO order { get; set; }
        public string thumbnail { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
    }
}
