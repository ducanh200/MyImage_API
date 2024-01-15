using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.Order;
using static System.Net.Mime.MediaTypeNames;

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
            try {
                List<Order> orders = _context.Orders.Include(p => p.User).OrderByDescending(o => o.Id).ToList();
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
                        email = o.User.Email,
                        phone = o.Phone,
                        address = o.Address,
                        city = o.City,
                        total_amount = o.TotalAmount,
                        status = o.Status,
                        created_at = Convert.ToDateTime(o.CreatedAt)
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
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
                    email = n.Email,
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
                        Email = model.email,
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
            var msgs = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
            return BadRequest(string.Join(" | ", msgs));
        }


        [HttpPut]
        [Route("UpdateOrder")]
        public IActionResult UpdateOrderStatus(int Id)
        {
            try
            {
                Order existingOrder = _context.Orders.Find(Id);

                if (existingOrder != null)
                {
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
                Order existingOrder = _context.Orders.Find(Id);

                if (existingOrder != null)
                {
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

        [HttpGet]
        [Route("get-order-cancelled")]
        public IActionResult GetOrdersCancelled()
        {
            try
            {
                List<Order> orders = _context.Orders
                    .Where(o => o.Status == 0)
                    .Include(o => o.User)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                if (orders.Count == 0)
                {
                    return BadRequest("No orders found with status 0.");
                }

                List<OrderDTO> data = new List<OrderDTO>();
                foreach (Order o in orders)
                {
                    data.Add(new OrderDTO
                    {
                        id = o.Id,
                        user_id = o.User.Id,
                        user = new UserDTO { id = o.User.Id, name = o.User.Name, email = o.User.Email, phone = o.User.Phone, address = o.User.Address, city = o.User.City },
                        email = o.User.Email,
                        phone = o.Phone,
                        address = o.Address,
                        city = o.City,
                        total_amount = o.TotalAmount,
                        status = o.Status,
                        created_at = Convert.ToDateTime(o.CreatedAt)
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders with status 0: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("get-order-waitting")]
        public IActionResult GetOrdersWaitting()
        {
            try
            {
                List<Order> orders = _context.Orders
                    .Where(o => o.Status == 1)
                    .Include(o => o.User)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                if (orders.Count == 0)
                {
                    return BadRequest("No orders found with status 1.");
                }

                List<OrderDTO> data = new List<OrderDTO>();
                foreach (Order o in orders)
                {
                    data.Add(new OrderDTO
                    {
                        id = o.Id,
                        user_id = o.User.Id,
                        user = new UserDTO { id = o.User.Id, name = o.User.Name, email = o.User.Email, phone = o.User.Phone, address = o.User.Address, city = o.User.City },
                        email = o.User.Email,
                        phone = o.Phone,
                        address = o.Address,
                        city = o.City,
                        total_amount = o.TotalAmount,
                        status = o.Status,
                        created_at = Convert.ToDateTime(o.CreatedAt)
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders with status 1: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("get-order-confirmed")]
        public IActionResult GetOrdersConfirmed()
        {
            try
            {
                List<Order> orders = _context.Orders
                    .Where(o => o.Status == 2)
                    .Include(o => o.User)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                if (orders.Count == 0)
                {
                    return BadRequest("No orders found with status 2.");
                }

                List<OrderDTO> data = new List<OrderDTO>();
                foreach (Order o in orders)
                {
                    data.Add(new OrderDTO
                    {
                        id = o.Id,
                        user_id = o.User.Id,
                        user = new UserDTO { id = o.User.Id, name = o.User.Name, email = o.User.Email, phone = o.User.Phone, address = o.User.Address, city = o.User.City },
                        email = o.User.Email,
                        phone = o.Phone,
                        address = o.Address,
                        city = o.City,
                        total_amount = o.TotalAmount,
                        status = o.Status,
                        created_at = Convert.ToDateTime(o.CreatedAt)
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders with status 2: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("get-order-shipping")]
        public IActionResult GetOrdersShipping()
        {
            try
            {
                List<Order> orders = _context.Orders
                    .Where(o => o.Status == 3)
                    .Include(o => o.User)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                if (orders.Count == 0)
                {
                    return BadRequest("No orders found with status 3.");
                }

                List<OrderDTO> data = new List<OrderDTO>();
                foreach (Order o in orders)
                {
                    data.Add(new OrderDTO
                    {
                        id = o.Id,
                        user_id = o.User.Id,
                        user = new UserDTO { id = o.User.Id, name = o.User.Name, email = o.User.Email, phone = o.User.Phone, address = o.User.Address, city = o.User.City },
                        email = o.User.Email,
                        phone = o.Phone,
                        address = o.Address,
                        city = o.City,
                        total_amount = o.TotalAmount,
                        status = o.Status,
                        created_at = Convert.ToDateTime(o.CreatedAt)
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders with status 3: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("get-order-shipped")]
        public IActionResult GetOrdersShipped()
        {
            try
            {
                List<Order> orders = _context.Orders
                    .Where(o => o.Status == 4)
                    .Include(o => o.User)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                if (orders.Count == 0)
                {
                    return BadRequest("No orders found with status 4.");
                }

                List<OrderDTO> data = new List<OrderDTO>();
                foreach (Order o in orders)
                {
                    data.Add(new OrderDTO
                    {
                        id = o.Id,
                        user_id = o.User.Id,
                        user = new UserDTO { id = o.User.Id, name = o.User.Name, email = o.User.Email, phone = o.User.Phone, address = o.User.Address, city = o.User.City },
                        email = o.User.Email,
                        phone = o.Phone,
                        address = o.Address,
                        city = o.City,
                        total_amount = o.TotalAmount,
                        status = o.Status,
                        created_at = Convert.ToDateTime(o.CreatedAt)
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders with status 4: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("get-order-successed")]
        public IActionResult GetOrdersSuccessed()
        {
            try
            {
                List<Order> orders = _context.Orders
                    .Where(o => o.Status == 5)
                    .Include(o => o.User)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                if (orders.Count == 0)
                {
                    return BadRequest("No orders found with status 5.");
                }

                List<OrderDTO> data = new List<OrderDTO>();
                foreach (Order o in orders)
                {
                    data.Add(new OrderDTO
                    {
                        id = o.Id,
                        user_id = o.User.Id,
                        user = new UserDTO { id = o.User.Id, name = o.User.Name, email = o.User.Email, phone = o.User.Phone, address = o.User.Address, city = o.User.City },
                        email = o.User.Email,
                        phone = o.Phone,
                        address = o.Address,
                        city = o.City,
                        total_amount = o.TotalAmount,
                        status = o.Status,
                        created_at = Convert.ToDateTime(o.CreatedAt)
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders with status 5: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("today-orders")]
        public IActionResult TodayOrders()
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = today.AddDays(1);

                List<Order> orders = _context.Orders
                    .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow)
                    .Include(o => o.User)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                if (orders.Count == 0)
                {
                    return BadRequest("No orders found created today.");
                }

                List<OrderDTO> data = new List<OrderDTO>();
                foreach (Order o in orders)
                {
                    data.Add(new OrderDTO
                    {
                        id = o.Id,
                        user_id = o.User.Id,
                        user = new UserDTO { id = o.User.Id, name = o.User.Name, email = o.User.Email, phone = o.User.Phone, address = o.User.Address, city = o.User.City },
                        email = o.User.Email,
                        phone = o.Phone,
                        address = o.Address,
                        city = o.City,
                        total_amount = o.TotalAmount,
                        status = o.Status,
                        created_at = Convert.ToDateTime(o.CreatedAt)
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders created today: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("today-orders-amount")]
        public IActionResult TodayOrdersAmount()
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = today.AddDays(1);

                int totalAmount = _context.Orders
                    .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow && o.Status != 0)
                    .Sum(o => o.TotalAmount);

                return Ok(new { total_amount = totalAmount });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving total amount of orders created today with status not equal to 0: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("yesterday-orders-amount")]
        public IActionResult YesterdayOrdersAmount()
        {
            try
            {
                DateTime yesterday = DateTime.Today.AddDays(-1);
                DateTime today = DateTime.Today;

                int totalAmount = _context.Orders
                    .Where(o => o.CreatedAt >= yesterday && o.CreatedAt < today && o.Status != 0)
                    .Sum(o => o.TotalAmount);

                return Ok(new { total_amount = totalAmount });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving total amount of orders created yesterday with status not equal to 0: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("2-days-ago-orders-amount")]
        public IActionResult TwoDaysAgoOrdersAmount()
        {
            try
            {
                DateTime twoDaysAgo = DateTime.Today.AddDays(-2);
                DateTime yesterday = DateTime.Today.AddDays(-1);

                int totalAmount = _context.Orders
                    .Where(o => o.CreatedAt >= twoDaysAgo && o.CreatedAt < yesterday && o.Status != 0)
                    .Sum(o => o.TotalAmount);

                return Ok(new { total_amount = totalAmount });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving total amount of orders created two days ago with status not equal to 0: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("3-days-ago-orders-amount")]
        public IActionResult ThreeDaysAgoOrdersAmount()
        {
            try
            {
                DateTime DaysAgo = DateTime.Today.AddDays(-3);
                DateTime timeday = DateTime.Today.AddDays(-2);

                int totalAmount = _context.Orders
                    .Where(o => o.CreatedAt >= DaysAgo && o.CreatedAt < timeday && o.Status != 0)
                    .Sum(o => o.TotalAmount);

                return Ok(new { total_amount = totalAmount });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving total amount of orders created days ago with status not equal to 0: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("4-days-ago-orders-amount")]
        public IActionResult FourDaysAgoOrdersAmount()
        {
            try
            {
                DateTime DaysAgo = DateTime.Today.AddDays(-4);
                DateTime timeday = DateTime.Today.AddDays(-3);

                int totalAmount = _context.Orders
                    .Where(o => o.CreatedAt >= DaysAgo && o.CreatedAt < timeday && o.Status != 0)
                    .Sum(o => o.TotalAmount);

                return Ok(new { total_amount = totalAmount });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving total amount of orders created days ago with status not equal to 0: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("5-days-ago-orders-amount")]
        public IActionResult FiveDaysAgoOrdersAmount()
        {
            try
            {
                DateTime DaysAgo = DateTime.Today.AddDays(-5);
                DateTime timeday = DateTime.Today.AddDays(-4);

                int totalAmount = _context.Orders
                    .Where(o => o.CreatedAt >= DaysAgo && o.CreatedAt < timeday && o.Status != 0)
                    .Sum(o => o.TotalAmount);

                return Ok(new { total_amount = totalAmount });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving total amount of orders created days ago with status not equal to 0: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("6-days-ago-orders-amount")]
        public IActionResult SixDaysAgoOrdersAmount()
        {
            try
            {
                DateTime DaysAgo = DateTime.Today.AddDays(-6);
                DateTime timeday = DateTime.Today.AddDays(-7);

                int totalAmount = _context.Orders
                    .Where(o => o.CreatedAt >= DaysAgo && o.CreatedAt < timeday && o.Status != 0)
                    .Sum(o => o.TotalAmount);

                return Ok(new { total_amount = totalAmount });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving total amount of orders created days ago with status not equal to 0: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("month-orders-amount/{month}")]
        public IActionResult MonthOrdersAmount(int month)
        {
            try
            {
                DateTime startOfMonth = new DateTime(DateTime.Now.Year, month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1);

                int totalAmount = _context.Orders
                    .Where(o => o.CreatedAt >= startOfMonth && o.CreatedAt < endOfMonth && o.Status != 0)
                    .Sum(o => o.TotalAmount);

                return Ok(new { total_amount = totalAmount });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving total amount of orders created in month {month} with status not equal to 0: {ex.Message}");
            }
        }

        // Tạo 12 hàm tượng trưng cho 12 tháng
        [HttpGet]
        [Route("month1-orders-amount")]
        public IActionResult Month1OrdersAmount() => MonthOrdersAmount(1);

        [HttpGet]
        [Route("month2-orders-amount")]
        public IActionResult Month2OrdersAmount() => MonthOrdersAmount(2);

        [HttpGet]
        [Route("month3-orders-amount")]
        public IActionResult Month3OrdersAmount() => MonthOrdersAmount(3);

        [HttpGet]
        [Route("month4-orders-amount")]
        public IActionResult Month4OrdersAmount() => MonthOrdersAmount(4);

        [HttpGet]
        [Route("month5-orders-amount")]
        public IActionResult Month5OrdersAmount() => MonthOrdersAmount(5);

        [HttpGet]
        [Route("month6-orders-amount")]
        public IActionResult Month6OrdersAmount() => MonthOrdersAmount(6);

        [HttpGet]
        [Route("month7-orders-amount")]
        public IActionResult Month7OrdersAmount() => MonthOrdersAmount(7);

        [HttpGet]
        [Route("month8-orders-amount")]
        public IActionResult Month8OrdersAmount() => MonthOrdersAmount(8);

        [HttpGet]
        [Route("month9-orders-amount")]
        public IActionResult Month9OrdersAmount() => MonthOrdersAmount(9);

        [HttpGet]
        [Route("month10-orders-amount")]
        public IActionResult Month10OrdersAmount() => MonthOrdersAmount(10);

        [HttpGet]
        [Route("month11-orders-amount")]
        public IActionResult Month11OrdersAmount() => MonthOrdersAmount(11);

        [HttpGet]
        [Route("month12-orders-amount")]
        public IActionResult Month12OrdersAmount() => MonthOrdersAmount(12);


        [HttpGet]
        [Route("get-orders-days-ago")]
        public IActionResult GetOrdersDaysAgo(int daysAgo)
        {
            try
            {
                DateTime startDate = DateTime.UtcNow.Date.AddDays(-daysAgo);
                DateTime endDate = DateTime.UtcNow.Date.AddDays(-(daysAgo - 1));

                List<Order> orders = _context.Orders
                    .Where(o => o.CreatedAt >= startDate && o.CreatedAt < endDate)
                    .Include(o => o.User)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                if (orders.Count == 0)
                {
                    return BadRequest($"No orders found created {daysAgo} days ago.");
                }

                List<OrderDTO> data = new List<OrderDTO>();
                foreach (Order o in orders)
                {
                    data.Add(new OrderDTO
                    {
                        id = o.Id,
                        user_id = o.User.Id,
                        user = new UserDTO { id = o.User.Id, name = o.User.Name, email = o.User.Email, phone = o.User.Phone, address = o.User.Address, city = o.User.City },
                        email = o.User.Email,
                        phone = o.Phone,
                        address = o.Address,
                        city = o.City,
                        total_amount = o.TotalAmount,
                        status = o.Status,
                        created_at = Convert.ToDateTime(o.CreatedAt)
                    });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving orders created {daysAgo} days ago: {ex.Message}");
            }
        }


    }
}
