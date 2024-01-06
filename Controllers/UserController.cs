using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IConfiguration _config;
        public UserController(MyimageContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string GenJWT(User user)
        {
            var secretkey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signatureKey = new SigningCredentials(secretkey,
                                    SecurityAlgorithms.HmacSha256);
            var payload = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Role,"user"),

            };
            var token = new JwtSecurityToken(
                    _config["JWT:Issuer"],
                    _config["JWT:Audience"],
                    payload,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signatureKey
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpGet]
        public IActionResult Index()
        {
            List<User> users = _context.Users.ToList();
            if (users.Count == 0)
            {
                return Ok("Không có tài khoản nào kho lưu trữ thông tin khách hàng !");
            }
            List<UserDTO> data = new List<UserDTO>();
            foreach (User c in users)
            {
                data.Add(new UserDTO { id = c.Id, name = c.Name, email = c.Email, phone = c.Phone, address = c.Address, city = c.City ,role =c.Role });
            }
            return Ok(users);
        }


        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserRegister model)
        {
            try
            {
                var saft = BCrypt.Net.BCrypt.GenerateSalt(10);
                var hashed = BCrypt.Net.BCrypt.HashPassword(model.password, saft);
                var user = new User
                {
                    Name = model.name,
                    Email = model.email,
                    Phone = model.phone,
                    Address = model.address,
                    City = model.city,
                    Role = "user",
                    Password = hashed
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok(new UserDTO
                {
                    id = user.Id,
                    email = user.Email,
                    name = user.Name,
                    phone = user.Phone,
                    address = user.Address,
                    city = user.City,
                    role = user.Role,
                    token = GenJWT(user)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Ngoại lệ nội bộ: {e.InnerException.Message}");
                }

                return Unauthorized("Đã xảy ra lỗi khi xử lý yêu cầu của bạn.");
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserLogin model)
        {
            try
            {
                var user = _context.Users.Where(u => u.Email.Equals(model.email))
                            .First();
                if (user == null)
                {
                    throw new Exception("Email or Password is not correct");
                }
                bool verified = BCrypt.Net.BCrypt.Verify(model.password, user.Password);
                if (!verified)
                {
                    throw new Exception("Email or Password is not correct");
                }
                return Ok(new UserDTO
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email,
                    phone = user.Phone,
                    address = user.Address,
                    city = user.City,
                    role = user.Role,
                    token = GenJWT(user)
                });

            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            // get info from token
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (!identity.IsAuthenticated)
            {
                return Unauthorized("Not Authorized");
            }
            try
            {
                var userClaims = identity.Claims;
                var userId = userClaims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = _context.Users.FirstOrDefault(c => c.Id == Convert.ToInt32(userId));
                return Ok(new UserDTO // đúng ra phải là UserProfileDTO
                {
                    id = user.Id,
                    email = user.Email,
                    name = user.Name,
                    phone = user.Phone,
                    address = user.Address,
                    city = user.City,
                    role = user.Role,
                   
                });
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}
