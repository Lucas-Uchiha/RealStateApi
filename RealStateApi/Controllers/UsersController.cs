using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealStateApi.Data;
using RealStateApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealStateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        ApiDbContext _dbContext = new ApiDbContext();
        private IConfiguration config;

        public UsersController(IConfiguration configuration)
        {
            config = configuration;
        }

        [HttpPost("[action]")]
        public IActionResult Register([FromBody] User user)
        {
            var exists = _dbContext.Users.FirstOrDefault(u => u.Email== user.Email) != null;    // check if user exists

            if (exists)
                return BadRequest();

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] User user)
        {
            var currentUser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

            if (currentUser == null)
                return NotFound();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.Email, user.Email) };

            var token = new JwtSecurityToken(
                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);
        }
    }
}
