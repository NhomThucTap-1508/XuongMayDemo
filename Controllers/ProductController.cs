using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace testthuctap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DBContextUser _context;

        public ProductController(DBContextUser context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet("GetAllProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Product.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("GetProductId")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public class ProductAddDto
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = null!;
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }
        public class ProductUpdateDto
        {
            public string ProductName { get; set; } = null!;
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
        }

        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdateDto productDto)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.ProductName = productDto.ProductName;
            product.Price = productDto.Price;
            product.CategoryID = productDto.CategoryId;
            int rowchange = await _context.SaveChangesAsync();
            if (rowchange > 0) {
                return Ok("Update product sucessful!!!");
            }
            return StatusCode(500, "Update product unsuccessful!!!");
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> PostProduct(ProductAddDto productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                CategoryID = productDto.CategoryId
            };

            _context.Product.Add(product);
            int rowchange = await _context.SaveChangesAsync();
            if (rowchange > 0)
            {
                return Ok("Create product sucessful!!!");
            }
            return StatusCode(500, "Create product unsuccessful!!!");
        }

        // DELETE: api/Products/5
        [HttpDelete("DeleteProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Delete product successful!!!");
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
