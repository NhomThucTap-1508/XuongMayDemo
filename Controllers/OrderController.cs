using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
namespace testthuctap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly DBContextUser _context;

        public OrderController(DBContextUser context)
        {
            _context = context;
        }

        // show Order
        [HttpGet("GetOrder")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Order.ToListAsync();
            return Ok(orders);
        }

        //show Order By ID
        [HttpGet("GetOrderById")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        //Pagination
        [HttpGet("Pagination")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<IActionResult> Pagination(int pageSize, int pageNumber)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                return BadRequest();
            }
            var skip = (pageNumber - 1) * pageSize;
            var ord = await _context.Order.Skip(skip).Take(pageSize).Select(l => new Order
            {
                OrderID = l.OrderID,
                ProductID = l.ProductID,
                Quantity = l.Quantity
            }).ToListAsync();
            return Ok(ord);
        }
        public class OrderNew
        {
            public int OrderID { get; set; }

            public int CategoryID { get; set; }

            public int ProductID { get; set; }

            public int Quantity { get; set; }
        }

        //Create New Order
        [HttpPost("CreateOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Order>> CreateOrder(OrderNew orderDTO)
        {
            var order = new Order
            {
                OrderID = orderDTO.OrderID,
                ProductID = orderDTO.ProductID,
                Quantity = orderDTO.Quantity
            };
            _context.Order.Add(order);
            int rowChange = await _context.SaveChangesAsync();
            if (rowChange > 0)
            {
                return Ok("Success");
            }
            return StatusCode(500, "Failed");
        }

        //Update Order
        [HttpPut("UpdateOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrder(int id, OrderNew orderDTO)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.ProductID = orderDTO.ProductID;
            order.Quantity = orderDTO.Quantity;
            _context.Order.Update(order);

            int rowChange = await _context.SaveChangesAsync();
            if (rowChange > 0)
            {
                return Ok("Success");
            }
            return StatusCode(500, "Update Failed!!!");
        }


        //Detele Order
        [HttpDelete("DeleteOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}