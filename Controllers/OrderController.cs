using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.Order;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IWebHostEnvironment _environment;

        public OrderController(MyimageContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Order> orders = _context.Orders.Include(p => p.User).ToList();
            if (orders.Count == 0)
            {
                return BadRequest("KHông có cơ sở dữ liệu !");
            }
            List<OrderDTO> data = new List<OrderDTO>();
            foreach (Order o in orders)
            {
                data.Add(new OrderDTO
                {
                    id = o.Id,
                    user_id = o.User.Id,
                    user = new UserDTO { id = o.User.Id, name = o.User.Name, email = o.User.Email, phone = o.User.Phone, address = o.User.Address, city = o.User.City },
                    phone = o.Phone,
                    address = o.Address,
                    city = o.City,
                    total_amount = o.TotalAmount,
                    status = o.Status,
                    created_at = Convert.ToDateTime(o.CreatedAt)
                });
            }

            return Ok(orders);
        }

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Order n = _context.Orders
                     .Where(n => n.Id == id).Include(n => n.User).First();
                if (n == null)
                    return NotFound();
                return Ok(new OrderDTO
                {
                    id = n.Id,
                    user_id = n.UserId,
                    user = new UserDTO { id = n.User.Id, name = n.User.Name, email = n.User.Email ,phone = n.User.Phone},
                    phone = n.Phone,
                    address = n.Address,
                    city = n.City,
                    total_amount = n.TotalAmount,
                    status = n.Status,
                    created_at = Convert.ToDateTime(n.CreatedAt)
                });

            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateOrder model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Order newOrder = new Order
                    {
                        UserId = model.user_id,
                        Phone = model.phone,
                        Address = model.address,
                        City = model.city,
                        TotalAmount = model.total_amount,
                        Status = 1,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();


                    return Ok(newOrder);
                }
                catch (Exception ex)
                {
                    return BadRequest("Đã xảy ra lỗi khi tạo order: " + ex.Message);
                }
            }

            return BadRequest("Dữ liệu không hợp lệ");
        }


        [HttpPut]
        [Route("UpdateOrder")]
        public IActionResult UpdateOrderStatus(int Id)
        {
            try
            {
                // Lấy đơn hàng từ cơ sở dữ liệu
                Order existingOrder = _context.Orders.Find(Id);

                // Kiểm tra xem đơn hàng tồn tại không (chưa hoàn thành)
                if (existingOrder != null)
                {
                    // Cập nhật trạng thái của đơn hàng
                    existingOrder.Status = existingOrder.Status + 1;
                    _context.SaveChanges();

                    return Ok(existingOrder);
                }

                return NotFound("Đơn hàng không tồn tại hoặc không thể cập nhật trạng thái.");
            }
            catch (Exception ex)
            {
                return BadRequest("Đã xảy ra lỗi khi cập nhật trạng thái đơn hàng: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("CancelOrder")]
        public IActionResult CancelOrder(int Id)
        {
            try
            {
                // Lấy đơn hàng từ cơ sở dữ liệu
                Order existingOrder = _context.Orders.Find(Id);

                // Kiểm tra xem đơn hàng tồn tại không (chưa hoàn thành)
                if (existingOrder != null)
                {
                    // Cập nhật trạng thái của đơn hàng
                    existingOrder.Status = 0;
                    _context.SaveChanges();

                    return Ok(existingOrder);
                }

                return NotFound("Đơn hàng không tồn tại hoặc không thể cập nhật trạng thái.");
            }
            catch (Exception ex)
            {
                return BadRequest("Đã xảy ra lỗi khi cập nhật trạng thái đơn hàng: " + ex.Message);
            }
        }




    }
}
