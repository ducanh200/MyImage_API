using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyImage_API.DTOs;
using MyImage_API.Entities;
using MyImage_API.Models.Admin;
using MyImage_API.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyImage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly MyimageContext _context;
        private readonly IConfiguration _config;
        public AdminController(MyimageContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string GenJWT(Admin admin)
        {
            var secretkey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signatureKey = new SigningCredentials(secretkey,
                                    SecurityAlgorithms.HmacSha256);
            var payload = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,admin.Id.ToString()),
                new Claim(ClaimTypes.Email,admin.Email),
                new Claim(ClaimTypes.Name,admin.Name),

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
            List<Admin> admins = _context.Admins.ToList();
            if (admins.Count == 0)
            {
                return Ok("Không có tài khoản nào kho lưu trữ thông tin khách hàng !");
            }
            List<AdminDTO> data = new List<AdminDTO>();
            foreach (Admin c in admins)
            {
                data.Add(new AdminDTO { id = c.Id, name = c.Name, email = c.Email});
            }
            return Ok(admins);
        }


        [HttpPost]
        [Route("register")]
        public IActionResult Register(AdminRegister model)
        {
            try
            {
                var saft = BCrypt.Net.BCrypt.GenerateSalt(10);
                var hashed = BCrypt.Net.BCrypt.HashPassword(model.password, saft);
                var admin = new Admin
                {
                    Name = model.name,
                    Email = model.email,
                    Password = hashed
                };
                _context.Admins.Add(admin);
                _context.SaveChanges();
                return Ok(new AdminDTO
                {
                    id = admin.Id,
                    email = admin.Email,
                    name = admin.Name,
                    token = GenJWT(admin)
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
        public IActionResult Login(AdminLogin model)
        {
            try
            {
                var admin = _context.Admins.Where(u => u.Email.Equals(model.email))
                            .First();
                if (admin == null)
                {
                    throw new Exception("Email or Password is not correct");
                }
                bool verified = BCrypt.Net.BCrypt.Verify(model.password, admin.Password);
                if (!verified)
                {
                    throw new Exception("Email or Password is not correct");
                }
                return Ok(new AdminDTO
                {
                    id = admin.Id,
                    name = admin.Name,
                    email = admin.Email,
                    token = GenJWT(admin)
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
                var adminClaims = identity.Claims;
                var adminId = adminClaims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier)?.Value;
                var admin = _context.Users.FirstOrDefault(c => c.Id == Convert.ToInt32(adminId));
                return Ok(new AdminDTO // đúng ra phải là UserProfileDTO
                {
                    id = admin.Id,
                    email = admin.Email,
                    name = admin.Name,
                });
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}

