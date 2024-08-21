using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace testthuctap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DBContextUser _context;

        public UserController(DBContextUser context)
        {
            _context = context;
        }

        // show User
        [HttpGet("GetUser")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        //show User by 
        [HttpGet("GetUserById")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
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
            var urs = await _context.Users.Skip(skip).Take(pageSize).Select(l => new ApplicationUser
            {
                UserName = l.UserName,
                Email = l.Email,
                PhoneNumber = l.PhoneNumber
            }).ToListAsync();
            return Ok(urs);
        }

        public class UserNew
        {
            public string UserName { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
        }

        //Create New User
        [HttpPost("CreateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> CreateUser(UserNew userDTO)
        {
            var user = new ApplicationUser
            {
                UserName = userDTO.UserName,
                PhoneNumber = userDTO.PhoneNumber,
                Email = userDTO.Email
            };
            _context.Users.Add(user);
            int rowChange = await _context.SaveChangesAsync();
            if (rowChange > 0)
            {
                return Ok("Success");
            }
            return StatusCode(500, "Failed");
        }

        public class UserUpdate
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
        }
        //Update User
        [HttpPut("UpdateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdate userDTO)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.PasswordHash = userDTO.Password;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.Email = userDTO.Email;
            _context.Users.Update(user);

            int rowChange = await _context.SaveChangesAsync();
            if (rowChange > 0)
            {
                return Ok("Success");
            }
            return StatusCode(500, "Update Failed!!!");
        }


        //Detele User
        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
