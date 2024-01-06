using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.Feedback;
using MyImage_API.Models.Hanger;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IWebHostEnvironment _environment;
        public FeedbackController(MyimageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<Feedback> feedbacks = _context.Feedbacks.Include(p => p.User).ToList();
            if (feedbacks.Count == 0)
            {
                return Ok("Không có dữ liệu nào được ghi !");
            }
            List<FeedbackDTO> data = new List<FeedbackDTO>();
            foreach (Feedback m in feedbacks)
            {
                data.Add(new FeedbackDTO 
                { 
                    id = m.Id, 
                    user_id = m.UserId, 
                    user = new UserDTO { 
                        id = m.User.Id,
                        name = m.User.Name,
                        email = m.User.Email 
                    },
                    message = m.Message, 
                    created_at = Convert.ToDateTime(m.CreatedAt)
                });
            }

            return Ok(feedbacks);
        }


        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Feedback m = _context.Feedbacks.Where(n => n.Id == id).Include(n => n.User).First();
                if (m != null)
                {
                    return Ok(new FeedbackDTO 
                    { 
                        id = m.Id, 
                        user_id = m.UserId,
                        user = new UserDTO { id = m.User.Id, name = m.User.Name, email = m.User.Email },
                        message = m.Message, 
                        created_at = Convert.ToDateTime(m.CreatedAt)
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NotFound();
        }


        [HttpPost]
        public IActionResult Create(CreateFeedback model)
        {
                try
                {
                    Feedback data = new Feedback
                    {
                        UserId = model.user_id,
                        Message = model.message,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Feedbacks.Add(data);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={data.Id}",
                    new FeedbackDTO
                    {
                        id = data.Id,
                        user_id = data.UserId,
                        message = data.Message,
                        created_at = Convert.ToDateTime(data.CreatedAt)
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            var msgs = ModelState.Values.SelectMany(v => v.Errors)
                   .Select(v => v.ErrorMessage);
        }

        [HttpPut]
        public IActionResult Update(EditFeedback model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Feedback feedback = new Feedback { Id = model.id, UserId = model.user_id, Message = model.message };
                    if (feedback != null)
                    {
                        _context.Feedbacks.Update(feedback);
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
                Feedback feedback = _context.Feedbacks.Find(id);
                if (feedback == null)
                    return NotFound();
                _context.Feedbacks.Remove(feedback);
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
