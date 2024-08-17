using Microsoft.AspNetCore.Mvc;
namespace testthuctap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok();
        }
    }
}