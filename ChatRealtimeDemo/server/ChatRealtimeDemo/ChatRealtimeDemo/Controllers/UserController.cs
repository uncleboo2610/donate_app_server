using ChatRealtimeDemo.Data;
using ChatRealtimeDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ChatRealtimeDemo.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserController : Controller
    {
        private readonly ChatRealtimeDbContext db;
        private readonly IConfiguration _configuration;

        public UserController(ChatRealtimeDbContext db, IConfiguration configuration) 
        {
            this.db = db;
            _configuration = configuration;
        }

        public static Users u = new Users();

        [Authorize]
        [HttpGet]
        [Route("get-users")]
        public IActionResult GetUsers()
        {
            return Ok(db.Users.ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("get-user")]
        public IActionResult GetUser(HttpContext context) 
        {
            if (context.Request.Headers.ContainsKey("User-Agent"))
            {
                var value = context.Request.Headers["User-Agent"];
                return Ok(value);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("add-user")]
        public async Task<IActionResult> AddUser(Users users)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(users.Password);

            var checkedUser = db.Users.Where(u => u.Email == users.Email).IsNullOrEmpty();

            if (checkedUser != true)
            {
                return BadRequest();
            }

            var user = new Users()
            {
                Email = users.Email,
                FullName = users.FullName,
                Password = hashedPassword,
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<Users> LogIn(LogInUser req)
        {
            if (req == null)
            {
                return BadRequest();
            }

            var user = db.Users.Where(u => u.Email == req.Email).FirstOrDefault();

            if (req.Email != user?.Email)
            {
                return BadRequest();
            }
            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
            {
                return BadRequest();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("userId", user.Id.ToString())
            };

            var token = this.CreateToken(claims);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPut]
        [Route("update-user/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, Users u)
        {
            var user = db.Users.Find(id);

            if (user != null)
            {
                user.FullName = u.FullName;
                user.Email = u.Email;
                user.Password = u.Password;

                await db.SaveChangesAsync();

                return Ok(user);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = db.Users.Find(id);

            if (user != null)
            {
                db.Remove(user);
                await db.SaveChangesAsync();

                return Ok(user);
            }

            return NotFound();
        }

        private JwtSecurityToken CreateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: cred
                );

            return token;
        }
    }
}
