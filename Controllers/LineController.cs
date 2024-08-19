using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace testthuctap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly DBContextUser _context;
        public LineController(DBContextUser context)
        {
            _context = context;
        }
        [HttpGet("ReadAllLine")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<ActionResult<IEnumerable<Line>>> GetAllLines()
        {
            return await _context.Line.ToListAsync();
        }
        [HttpGet("ReadLineById")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<ActionResult<Line>> GetLineById(int lineId)
        {
            var line = await _context.Line.FindAsync(lineId);
            if (line == null)
            {
                return NotFound("Cannot find this line id");
            }
            return line;
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
            var lines = await _context.Line.Skip(skip).Take(pageSize).Select(l => new Line
            {
                LineID = l.LineID,
                LineName = l.LineName,
                Id = l.Id
            }).ToListAsync();
            return Ok(lines);
        }
        public class LineCreateDto
        {
            public string LineName { get; set; }
            public string Id { get; set; }
        }
        public class LineUpdateDto
        {
            public string LineName { get; set; }
        }
        [HttpPost("CreateLine")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Line>> PostLine(LineCreateDto lineCreateDto)
        {
            var line = new Line
            {
                LineName = lineCreateDto.LineName,
                Id = lineCreateDto.Id
            };
            _context.Line.Add(line);
            int rowChange = await _context.SaveChangesAsync();
            if (rowChange > 0)
            {
                return Ok("Create new line successful");
            }
            return StatusCode(500, "Create new line failed");
        }
        [HttpPut("UpdateLine")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutLine(int lineId, LineUpdateDto lineUpdateDto)
        {
            var line = await _context.Line.FindAsync(lineId);
            if (line == null)
            {
                return NotFound();
            }

            line.LineName = lineUpdateDto.LineName;
            _context.Line.Update(line);
            int rowChange = await _context.SaveChangesAsync();
            if (rowChange > 0)
            {
                return Ok("Update line successful");
            }
            return StatusCode(500, "Update line failed");
        }
        [HttpDelete("DeleteLine")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLine(int lineId)
        {
            var line = await _context.Line.FindAsync(lineId);
            if (line == null)
            {
                return NotFound();
            }

            _context.Line.Remove(line);
            await _context.SaveChangesAsync();
            return StatusCode(500, "Delete successful");
        }
    }
}
