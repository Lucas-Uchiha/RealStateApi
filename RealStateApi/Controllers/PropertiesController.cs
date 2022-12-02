using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApi.Data;
using RealStateApi.Models;
using System.Security.Claims;

namespace RealStateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        ApiDbContext dbContext = new ApiDbContext();

        [HttpGet]
        [Authorize]
        public IActionResult GetProperties(int categoryId) {
            var prop = dbContext.Properties.Where(c => c.CategoryId == categoryId);

            if(prop == null)
                return NotFound();

            return Ok(prop);
        }

        [HttpGet("PropertyDetail")]
        [Authorize]
        public IActionResult GetPropertyDetail(int id)
        {
            var prop = dbContext.Properties.FirstOrDefault(p => p.Id == id);

            if (prop == null)
                return NotFound();

            return Ok(prop);
        }

        [HttpGet("TrendingProperties")]
        [Authorize]
        public IActionResult GetTrendingProperties()
        {
            var prop = dbContext.Properties.Where(p => p.IsTrending);

            if (prop == null)
                return NotFound();

            return Ok(prop);
        }

        [HttpGet("SearchProperties")]
        [Authorize]
        public IActionResult GetSearchProperties(string address)
        {
            var prop = dbContext.Properties.Where(p => p.Address.Contains(address));

            if (prop == null)
                return NotFound();

            return Ok(prop);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Property property)
        {
            if(property == null)
                return NoContent();

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if(user == null)
                return NotFound();

            property.IsTrending = false;
            property.UserId= user.Id;

            dbContext.Properties.Add(property);
            dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] Property property)
        {
            var prop = dbContext.Properties.FirstOrDefault(p => p.Id == id);

            if (prop == null)
                return NotFound();

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
                return NotFound();

            if (prop.UserId != user.Id)
                return BadRequest();

            prop.Name = property.Name;
            prop.Details = property.Details;
            prop.Price = property.Price;
            prop.Address = property.Address;
            property.IsTrending = false;
            property.UserId = user.Id;

            dbContext.SaveChanges();

            return Ok("Record updated.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var prop = dbContext.Properties.FirstOrDefault(p => p.Id == id);

            if (prop == null)
                return NotFound();

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
                return NotFound();

            if (prop.UserId != user.Id)
                return BadRequest();

            dbContext.Properties.Remove(prop);
            dbContext.SaveChanges();

            return Ok("Record deleted.");
        }
    }
}
