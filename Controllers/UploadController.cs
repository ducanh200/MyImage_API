using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyImage_API.Entities;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IWebHostEnvironment _environment;
        public UploadController(MyimageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index(IFormFile image)
        {
            return Ok();
        }

        [HttpPost("upload")]
        public IActionResult UploadImage(IFormFile file)
        {

            if (file != null && file.Length > 0)
            {
                try
                {
                    // Tạo đường dẫn lưu trữ file trong thư mục "uploads"
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                    // Đảm bảo thư mục tồn tại
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Tạo tên file duy nhất để tránh trùng lặp
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Kết hợp đường dẫn thư mục với tên file
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Lưu file vào đường dẫn
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Trả về thông báo hoặc đường dẫn file đã lưu trữ
                    return Ok($"File {uniqueFileName} đã được tải lên thành công.");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Lỗi khi tải lên: {ex.Message}");
                }
            }
            else
            {
                return BadRequest("File không hợp lệ hoặc trống rỗng.");
            }
        }
    }
}
