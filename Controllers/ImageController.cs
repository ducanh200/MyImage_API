using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.Image;

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
            if(images.Count == 0)
            {
                return Ok("Không dữ liệu !");
            }
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

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Image i = _context.Images.Where(i => i.Id == id).First();
                if (i == null)
                    return NotFound();
                return Ok(new ImageDTO
                {
                    id = i.Id,
                    frame_id = i.FrameId,
                    material_id = i.MaterialId,
                    size_id = i.SizeId,
                    thumbnail = i.Thumbnail,
                    quantity = i.Quantity,
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Create([FromForm] CreateImage model )
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if(model.thumbnail != null && model.thumbnail.Length >0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.thumbnail.FileName);
                        var filePath = Path.Combine("uploads", fileName);

                        var absolutePath = Path.Combine(_environment.WebRootPath, filePath);

                        using (var fileStream = new FileStream(absolutePath, FileMode.Create))
                        {
                            model.thumbnail.CopyTo(fileStream);
                        }

                        string url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                        var frame = _context.Frames.Find(model.frame_id);
                        var material = _context.Materials.Find(model.material_id);
                        var size = _context.Sizes.Find(model.size_id);

                        Image image = new Image
                        {
                            FrameId = model.frame_id,
                            MaterialId = model.material_id,
                            SizeId = model.size_id,
                            Thumbnail = url,
                            Quantity = model.quantity,
                        };

                        _context.Images.Add(image);
                        _context.SaveChanges();

                        ImageDTO imageDTO = new ImageDTO
                        {
                            id = image.Id,
                            frame_id = image.FrameId,
                            material_id = image.MaterialId,
                            size_id = image.SizeId,
                            thumbnail = image.Thumbnail,
                            quantity = image.Quantity,
                        };

                        return Created(url, imageDTO);
                    }
                    else
                    {
                        return BadRequest("Vui lòng chọn file ảnh");
                    }

                }catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var msgs = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
            return BadRequest(string.Join(" | ", msgs));
        }


        [HttpPut]
        public IActionResult Update([FromForm] EditImage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Tìm kiếm bản ghi News để cập nhật
                    Image existingImage = _context.Images.Find(model.id);

                    if (existingImage == null)
                    {
                        return NotFound("Không tìm thấy bài báo cần sửa");
                    }

                    // Kiểm tra xem model có chứa ảnh mới hay không
                    if (model.thumbnail != null && model.thumbnail.Length > 0)
                    {
                        // Tạo đường dẫn lưu trữ file trong thư mục "uploads"
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                        // Đảm bảo thư mục tồn tại
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Tạo tên file duy nhất để tránh trùng lặp
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.thumbnail.FileName);

                        // Kết hợp đường dẫn thư mục với tên file
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        // Lưu file vào đường dẫn
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.thumbnail.CopyTo(fileStream);
                        }
                        string url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                        // Cập nhật đường dẫn ảnh mới
                        existingImage.Thumbnail = url;
                    }

                    // Cập nhật các thuộc tính khác từ model
                    existingImage.FrameId = model.frame_id;
                    existingImage.MaterialId = model.material_id;
                    existingImage.SizeId = model.size_id;
                    existingImage.Quantity = model.quantity;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();

                    return Ok("Đã sửa bài báo thành công!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Dữ liệu không hợp lệ");
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                Image image = _context.Images.Find(id);
                if (image == null)
                    return NotFound();
                _context.Images.Remove(image);
                _context.SaveChanges();
                return Ok("Deleted");

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
