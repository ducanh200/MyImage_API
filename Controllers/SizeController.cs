using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.Size;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly MyimageContext _context;

        public SizeController(MyimageContext context)
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult Index()
        {
            List<Size> sizes = _context.Sizes.ToList();
            if (sizes.Count == 0)
            {
                return Ok("Không có dữ liệu nào được ghi !");
            }
            List<SizeDTO> data = new List<SizeDTO>();
            foreach (Size s in sizes)
            {
                data.Add(new SizeDTO { id = s.Id, size_name = s.SizeName, size_amount = s.SizeAmount, size_width = s.SizeWidth });
            }
            return Ok(sizes);
        }


        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Size s = _context.Sizes.Find(id);
                if (s != null)
                {
                    return Ok(new SizeDTO { id = s.Id, size_name = s.SizeName, size_amount = s.SizeAmount, size_width = s.SizeWidth });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NotFound();
        }


        [HttpPost]
        public IActionResult Create(CreateSize model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Size data = new Size { SizeName = model.size_name, SizeAmount = model.size_amount, SizeWidth = model.size_width};
                    _context.Sizes.Add(data);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={data.Id}",
                    new SizeDTO { id = data.Id, size_name = data.SizeName, size_amount = data.SizeAmount, size_width = data.SizeWidth});
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            var msgs = ModelState.Values.SelectMany(v => v.Errors)
                   .Select(v => v.ErrorMessage);
            return BadRequest(string.Join(", ", msgs));
        }

        [HttpPut]
        public IActionResult Update(EditSize model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Size size = new Size { Id = model.id, SizeName = model.size_name, SizeAmount = model.size_amount, SizeWidth = model.size_width };
                    if (size != null)
                    {
                        _context.Sizes.Update(size);
                        _context.SaveChanges();
                        return Ok("Đổi thành công tên danh mục!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                Size size = _context.Sizes.Find(id);
                if (size == null)
                    return NotFound();
                _context.Sizes.Remove(size);
                _context.SaveChanges();
                return Ok("Đã xóa thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
