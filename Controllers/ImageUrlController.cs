using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.Image;
using MyImage_API.Models.Url;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUrlController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IWebHostEnvironment _environment;
        public ImageUrlController(MyimageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ImageUrl> imageUrls = _context.ImageUrls.ToList();
            if (imageUrls.Count == 0)
            {
                return Ok("Không dữ liệu !");
            }
            List<ImageUrlDTO> data = new List<ImageUrlDTO>();
            foreach (ImageUrl n in imageUrls)
            {
                data.Add(new ImageUrlDTO
                {
                    id = n.Id,
                    thumbnail = n.Thumbnail,
                });
            }
            return Ok(data);

        }

        [HttpGet("latest")]
        public IActionResult GetLatestImageUrl()
        {
            try
            {
                // Lấy thông tin của ImageUrl có id lớn nhất
                var latestImageUrl = _context.ImageUrls.OrderByDescending(x => x.Id).FirstOrDefault();

                if (latestImageUrl == null)
                {
                    return NotFound("Không tìm thấy ImageUrl");
                }

                // Chuyển đổi sang DTO nếu cần
                var imageUrlDTO = new ImageUrlDTO
                {
                    id = latestImageUrl.Id,
                    thumbnail = latestImageUrl.Thumbnail,
                };

                return Ok(imageUrlDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        public IActionResult Create([FromForm] CreateUrl model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.thumbnail != null && model.thumbnail.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.thumbnail.FileName);
                        var filePath = Path.Combine("uploads", fileName);

                        var absolutePath = Path.Combine(_environment.WebRootPath, filePath);

                        using (var fileStream = new FileStream(absolutePath, FileMode.Create))
                        {
                            model.thumbnail.CopyTo(fileStream);
                        }

                        string url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";


                        ImageUrl image = new ImageUrl
                        {

                            Thumbnail = url,

                        };

                        _context.ImageUrls.Add(image);
                        _context.SaveChanges();

                        ImageUrlDTO imageUrlDTO = new ImageUrlDTO
                        {
                            id = image.Id,
                            thumbnail = image.Thumbnail,

                        };

                        return Created(url, imageUrlDTO);
                    }
                    else
                    {
                        return BadRequest("Vui lòng chọn file ảnh");
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var msgs = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
            return BadRequest(string.Join(" | ", msgs));
        }
    }
}
