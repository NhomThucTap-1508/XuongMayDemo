using Microsoft.AspNetCore.Mvc;

namespace testthuctap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok("ProductController");
        }
    }
}
