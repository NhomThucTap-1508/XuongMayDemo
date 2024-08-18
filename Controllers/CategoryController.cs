using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testthuctap.data.migrations;


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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Category.ToListAsync();
    }

    // GET: api/Categories/5: Lấy 1 trong các loại sản phẩm
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Category.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    // PUT: api/Categories/5: Thêm mới 1 loại sản phẩm
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    public class CategoryAddDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, CategoryAddDto categoryDto)
    {
        var category = new Category
        {
            CategoryID = categoryDto.CategoryId,
            CategoryName = categoryDto.CategoryName
        };
        if (id != category.CategoryID)
        {
            return BadRequest();
        }

        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Categories: Cập nhật loại sản phẩm
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(CategoryAddDto categoryDto)
    {
        var category = new Category
        {
            CategoryID = categoryDto.CategoryId,
            CategoryName = categoryDto.CategoryName
        };
        _context.Category.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCategory", new { id = category.CategoryID }, category);
    }

    // DELETE: api/Categories/5: Xóa 1 loại sản phẩm
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Category.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(int id)
    {
        return _context.Category.Any(e => e.CategoryID == id);
    }
}