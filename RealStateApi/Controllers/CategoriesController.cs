using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApi.Data;

namespace RealStateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ApiDbContext dbContext = new ApiDbContext();

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(dbContext.Categories);
        }
    }
}
