namespace MyImage_API.DTOs
{
    public class ImageDTO
    {
        public int id {  get; set; }
        public int frame_id { get; set; }
        public int material_id { get; set; }
        public int size_id { get; set; }
        public int order_id { get; set; }
        public string thumbnail { get; set; }
        public int quantity { get; set; }
    }
}
