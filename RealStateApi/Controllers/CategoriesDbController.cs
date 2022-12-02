using Microsoft.AspNetCore.Mvc;
using RealStateApi.Data;
using RealStateApi.Models;

namespace RealStateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesDbController : ControllerBase
    {
        ApiDbContext _dbContext = new ApiDbContext();

        // GET: api/<CategoriesDbController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Categories);
        }

        // GET api/<CategoriesDbController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category =  _dbContext.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound("No record found for " + id);

            return Ok(category);
        }

        // GET api/<CategoriesDbController>/SortCategories
        [HttpPost("[action]")]
        public IActionResult SortCategories()
        {
            return Ok(_dbContext.Categories.OrderByDescending(x => x.Name));
        }

        // POST api/<CategoriesDbController>
        [HttpPost]
        public IActionResult Post([FromBody] Category value)
        {
            _dbContext.Categories.Add(value);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<CategoriesDbController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category value)
        {
            var cat = _dbContext.Categories.Find(id);

            if (cat == null)
                return NotFound("No record found for id: " + id);

            cat.Name = value.Name;
            cat.ImageUrl = value.ImageUrl;
            _dbContext.SaveChanges();

            return Ok("Successfully updated.");
        }

        // DELETE api/<CategoriesDbController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cat = _dbContext.Categories.Find(id);

            if (cat == null)
                return NotFound("No record found for id: " + id);

            _dbContext.Categories.Remove(cat);
            _dbContext.SaveChanges();

            return Ok("Deleted.");
        }
    }
}
