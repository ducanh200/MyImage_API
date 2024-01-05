using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.Hanger;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangerController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IWebHostEnvironment _environment;
        public HangerController(MyimageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [HttpGet]

        public IActionResult Index()
        {
            List<Hanger> hangers = _context.Hangers.ToList();
            if (hangers.Count == 0)
            {
                return Ok("Không có dữ liệu nào được ghi !");
            }
            List<HangerDTO> data = new List<HangerDTO>();
            foreach (Hanger m in hangers)
            {
                data.Add(new HangerDTO { id = m.Id, hanger_amount = m.HangerAmount, hanger_name = m.HangerName });
            }
            return Ok(hangers);
        }


        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Hanger m = _context.Hangers.Find(id);
                if (m != null)
                {
                    return Ok(new HangerDTO { id = m.Id, hanger_amount = m.HangerAmount, hanger_name = m.HangerName });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NotFound();
        }


        [HttpPost]
        public IActionResult Create(CreateHanger model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Hanger data = new Hanger
                    {
                        HangerAmount = model.hanger_amount,
                        HangerName = model.hanger_name
                    };
                    _context.Hangers.Add(data);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={data.Id}",
                    new HangerDTO
                    {
                        id = data.Id,
                        hanger_amount = data.HangerAmount,
                        hanger_name = data.HangerName
                    });
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
        public IActionResult Update(EditHanger model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Hanger hanger = new Hanger { Id = model.id, HangerAmount = model.hanger_amount, HangerName = model.hanger_name };
                    if (hanger != null)
                    {
                        _context.Hangers.Update(hanger);
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
                Hanger hanger = _context.Hangers.Find(id);
                if (hanger == null)
                    return NotFound();
                _context.Hangers.Remove(hanger);
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

