using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("/api/[Controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok("Hello world!");
    }
}