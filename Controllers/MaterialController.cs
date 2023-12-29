using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.Material;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IWebHostEnvironment _environment;
        public MaterialController(MyimageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [HttpGet]

        public IActionResult Index()
        {
            List<Material> materials = _context.Materials.ToList();
            if (materials.Count == 0)
            {
                return Ok("Không có dữ liệu nào được ghi !");
            }
            List<MaterialDTO> data = new List<MaterialDTO>();
            foreach (Material m in materials)
            {
                data.Add(new MaterialDTO { id = m.Id, material_amount = m.MaterialAmount, material_name = m.MaterialName });
            }
            return Ok(materials);
        }


        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Material m = _context.Materials.Find(id);
                if (m != null)
                {
                    return Ok(new MaterialDTO { id = m.Id, material_amount = m.MaterialAmount, material_name = m.MaterialName });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NotFound();
        }


        [HttpPost]
        public IActionResult Create(CreateMaterial model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Material data = new Material
                    { 
                        MaterialAmount = model.material_amount, 
                        MaterialName = model.material_name
                    };
                    _context.Materials.Add(data);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={data.Id}",
                    new MaterialDTO 
                    { 
                        id = data.Id, 
                        material_amount = data.MaterialAmount, 
                        material_name = data.MaterialName
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
        public IActionResult Update(EditMaterial model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Material material = new Material { Id = model.id, MaterialAmount = model.material_amount, MaterialName = model.material_name};
                    if (material != null)
                    {
                        _context.Materials.Update(material);
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
                Material material = _context.Materials.Find(id);
                if (material == null)
                    return NotFound();
                _context.Materials.Remove(material);
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
