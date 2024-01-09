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
    public class OrderImageController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IWebHostEnvironment _environment;
        public OrderImageController(MyimageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<OrderImage> orderImages = _context.OrderImages.Include(p => p.Size).Include(p => p.Frame).Include(p => p.Hanger).Include(p =>p.Order).ToList();
            if (orderImages.Count == 0)
            {
                return Ok("Không dữ liệu !");
            }
            List<OrderImageDTO> data = new List<OrderImageDTO>();
            foreach (OrderImage n in orderImages)
            {   
                data.Add(new OrderImageDTO
                {
                    id = n.Id,
                    frame_id = n.FrameId,
                    frame = new FrameDTO { id = n.Frame.Id, frame_amount = n.Frame.FrameAmount, frame_name = n.Frame.FrameName, frame_color_outsite = n.Frame.FrameColorOutsite, frame_color_insite = n.Frame.FrameColorInsite },
                    hanger_id = n.HangerId,
                    hanger = new HangerDTO { id = n.Hanger.Id, hanger_amount = n.Hanger.HangerAmount, hanger_name = n.Hanger.HangerName },
                    size_id = n.SizeId,
                    size = new SizeDTO { id = n.Size.Id, size_amount = n.Size.SizeAmount, size_name = n.Size.SizeName, size_width = n.Size.SizeWidth },
                    order_id = n.OrderId,
                    order = new OrderDTO { id = n.Order.Id , user_id = n.Order.UserId , email = n.Order.Email, phone = n.Order.Phone, address = n.Order.Address, city = n.Order.City, total_amount = n.Order.TotalAmount, status = n.Order.Status, created_at = n.Order.CreatedAt },
                    thumbnail = n.Thumbnail,
                    quantity = n.Quantity,
                    amount = n.Amount
                });
            }
            return Ok(data);

        }

        [HttpGet]
        [Route("get-by-order-id")]
        public IActionResult GetByOrderId(int orderId)
        {
            try
            {
                List<OrderImage> orderImages = _context.OrderImages
                    .Where(i => i.OrderId == orderId)
                    .Include(p => p.Size)
                    .Include(p => p.Frame)
                    .Include(p => p.Hanger)
                    .Include(p => p.Order)
                    .ToList();

                if (orderImages.Count == 0)
                    return NotFound();

                List<OrderImageDTO> data = new List<OrderImageDTO>();

                foreach (OrderImage i in orderImages)
                {
                    data.Add(new OrderImageDTO
                    {
                        id = i.Id,
                        frame_id = i.FrameId,
                        frame = new FrameDTO { id = i.Frame.Id, frame_amount = i.Frame.FrameAmount, frame_name = i.Frame.FrameName, frame_color_outsite = i.Frame.FrameColorOutsite, frame_color_insite = i.Frame.FrameColorInsite },
                        hanger_id = i.HangerId,
                        hanger = new HangerDTO { id = i.Hanger.Id, hanger_amount = i.Hanger.HangerAmount, hanger_name = i.Hanger.HangerName },
                        size_id = i.SizeId,
                        size = new SizeDTO { id = i.Size.Id, size_amount = i.Size.SizeAmount, size_name = i.Size.SizeName, size_width = i.Size.SizeWidth },
                        order_id = i.OrderId,
                        order = new OrderDTO { id = i.Order.Id, user_id = i.Order.UserId, email = i.Order.Email, phone = i.Order.Phone, address = i.Order.Address, city = i.Order.City, total_amount = i.Order.TotalAmount, status = i.Order.Status, created_at = i.Order.CreatedAt },
                        thumbnail = i.Thumbnail,
                        quantity = i.Quantity,
                        amount = i.Amount
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        public IActionResult Create([FromForm] CreateImage model)
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

                        var frame = _context.Frames.Find(model.frame_id);
                        var hanger = _context.Hangers.Find(model.hanger_id);
                        var size = _context.Sizes.Find(model.size_id);

                        OrderImage image = new OrderImage
                        {
                            FrameId = model.frame_id,
                            HangerId = model.hanger_id,
                            SizeId = model.size_id,
                            OrderId = model.order_id,
                            Thumbnail = url,
                            Quantity = model.quantity,
                            Amount = model.amount,
                        };

                        _context.OrderImages.Add(image);
                        _context.SaveChanges();

                        OrderImageDTO orderImageDTO = new OrderImageDTO
                        {
                            id = image.Id,
                            frame_id = image.FrameId,
                            hanger_id = image.HangerId,
                            size_id = image.SizeId,
                            order_id = image.OrderId,
                            thumbnail = image.Thumbnail,
                            quantity = image.Quantity,
                            amount = image.Amount,
                        };

                        return Created(url, orderImageDTO);
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


       /* [HttpPut]
        public IActionResult Update([FromForm] EditImage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Tìm kiếm bản ghi News để cập nhật
                    OrderImage existingImage = _context.OrderImages.Find(model.id);

                    if (existingImage == null)
                    {
                        return NotFound("Không tìm thấy ảnh cần sửa");
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
                    existingImage.HangerId = model.hanger_id;
                    existingImage.SizeId = model.size_id;
                    existingImage.Quantity = model.quantity;
                    existingImage.Amount = model.amount;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();

                    return Ok("Đã sửa thành công!");
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
                OrderImage image = _context.OrderImages.Find(id);
                if (image == null)
                    return NotFound();
                _context.OrderImages.Remove(image);
                _context.SaveChanges();
                return Ok("Deleted");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}
