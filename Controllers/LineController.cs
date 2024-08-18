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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Line>>> GetAllLines()
        {
            return await _context.Line.ToListAsync();
        }
        public class LineDto
        {
            public int LineId { get; set; }
            public string LineName { get; set; }
            public string Id { get; set; }
        }
        [HttpPost]
        public async Task<ActionResult<Line>> PostLine(LineDto lineDto)
        {
            var line = new Line
            {
                LineID = lineDto.LineId,
                LineName = lineDto.LineName,
                Id = lineDto.Id
            };
            _context.Line.Add(line);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLine", new { id = line.LineID }, line);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLine(int id, LineDto lineDto)
        {
            if (id != lineDto.LineId)
            {
                return BadRequest();
            }

            var line = await _context.Line.FindAsync(id);
            if (line == null)
            {
                return NotFound();
            }

            line.LineName = lineDto.LineName;
            line.Id = lineDto.Id;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return Ok("Update successful");
                }
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLine(int id)
        {
            var line = await _context.Line.FindAsync(id);
            if (line == null)
            {
                return NotFound();
            }

            _context.Line.Remove(line);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool LineExists(int id)
        {
            return _context.Line.Any(e => e.LineID == id);
        }
    }
}
