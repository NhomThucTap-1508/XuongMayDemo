using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



// cấu hình controller 
[ApiController]
[Route("/api/[Controller]")]
public class CategoryController : ControllerBase
{

    private readonly DBContextUser _context;

    public CategoryController(DBContextUser context)
    {
        _context = context;
    }

    // GET: api/Categories: Lấy toàn bộ danh sách loại sản phẩm
    [HttpGet("GetAllCategory")]
    [Authorize(Roles = "Admin,LineLeader")]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Category.ToListAsync();
    }

    // GET: api/Categories/5: Lấy 1 trong các loại sản phẩm
    [HttpGet("GetCategoryId")]
    [Authorize(Roles = "Admin,LineLeader")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Category.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }
    [HttpGet("GetCategoryByName")]
    [Authorize(Roles = "Admin,LineLeader")]
    public async Task<ActionResult<Category>> GetCategory(string name)
    {
        var category = await _context.Category.FirstOrDefaultAsync(c => c.CategoryName == name);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpGet("Pagination")]
    [Authorize(Roles = "Admin,LineLeader")]
    public async Task<IActionResult> Pagination(int pageSize, int pageNumber)
    {
        if (pageSize <= 0 || pageNumber <= 0)
        {
            return BadRequest("PageSize and PageNumber must be greater than zero");
        }
        var skip = (pageNumber - 1) * pageSize;
        var category = await _context.Category.Skip(skip).Take(pageSize).Select(c => new Category
        {
            CategoryID = c.CategoryID,
            CategoryName = c.CategoryName
        }).ToListAsync();
        return Ok(category);
    }
    // PUT: api/Categories/5: Thêm mới 1 loại sản phẩm
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    public class CategoryAddDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;
    }
    [HttpPut("UpdateCategory")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutCategory(int id, CategoryAddDto categoryDto)
    {
        var category = await _context.Category.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        category.CategoryName = categoryDto.CategoryName;
        _context.Category.Update(category);
        int rowChange = await _context.SaveChangesAsync();
        if (rowChange > 0)
        {
            return Ok("Update category successful!!!");
        }
        return StatusCode(500, "Update category unsuccessful!!!");
    }

    // POST: api/Categories: Cập nhật loại sản phẩm
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("CreateCategory")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Category>> PostCategory(CategoryAddDto categoryDto)
    {
        var category = new Category
        {
            CategoryID = categoryDto.CategoryId,
            CategoryName = categoryDto.CategoryName
        };
        _context.Category.Add(category);
        int rowChange = await _context.SaveChangesAsync();
        if (rowChange > 0)
        {
            return Ok("Create category sucessful!!!");
        }
        return StatusCode(500, "Create category unsuccessful!!!");

    }

    // DELETE: api/Categories/5: Xóa 1 loại sản phẩm
    [HttpDelete("DeleteCategory")]
    [Authorize(Roles = "Admin,LineLeader")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Category.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();

        return Ok("Deleto successful!!");
    }

    private bool CategoryExists(int id)
    {
        return _context.Category.Any(e => e.CategoryID == id);
    }
}