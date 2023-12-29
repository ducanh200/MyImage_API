using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImage_API.DTOs;
using MyImage_API.Entities;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IWebHostEnvironment _environment;
        public ImageController(MyimageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<Image> images = _context.Images.Include(p => p.Size).Include(p => p.Frame).Include(p => p.Material).ToList();
            List<ImageDTO> data = new List<ImageDTO>();
            foreach (Image n in images)
            {
                data.Add(new ImageDTO
                {
                    id = n.Id,
                    frame_id = n.FrameId,
                    material_id = n.MaterialId,
                    size_id = n.SizeId,
                    thumbnail = n.Thumbnail,
                    quantity = n.Quantity,
                });
            }
            return Ok(data);

        }

        
    }
}
