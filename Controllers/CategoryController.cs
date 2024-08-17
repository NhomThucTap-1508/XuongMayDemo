using Microsoft.AspNetCore.Mvc;


// cấu hình controller 
[ApiController]
[Route("/api/[Controller]")]
public class CategoryController : ControllerBase
{
    /// khai báo service
    private readonly CategoryService categoryService;
    public CategoryController(CategoryService categoryService)
    {
        this.categoryService = categoryService;
    }
    // HTTPGET cho phương thức 
    [HttpGet("GetAllCategory")]
    public async Task<IActionResult> GetAll()
    {
        IList<Category> categories = await categoryService.GetCategories();
        // trả về danh sách các category với status 200
        return Ok(categories);
    }

}