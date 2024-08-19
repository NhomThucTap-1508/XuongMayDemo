using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace testthuctap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }
        [HttpGet("GetAllTask")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<IActionResult> GetAllTask()
        {
            var tasks = await taskService.GetTasks();
            return Ok(tasks);
        }
        [HttpPost("CreateTask")]
        [Authorize(Roles = "Admin,LineLeader")]

        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel createTaskModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userName = User.FindFirstValue(ClaimTypes.Name);

            int rowChange = await taskService.CreateTask(createTaskModel, userName);
            if (rowChange > 0)
            {
                return Ok("Tạo thành công thành công");
            }
            return StatusCode(500, "Lỗi server");
        }
        [HttpPut("UpdateTask")]
        [Authorize(Roles = "Admin,LineLeader")]

        public async Task<IActionResult> UpdateTask([FromQuery] int taskId, [FromBody] UpdateTaskModel updateTaskModel)
        {
            int rowChange = await taskService.UpdateTask(taskId, updateTaskModel);
            if (rowChange > 0)
            {
                return Ok("Cập nhật thành công");
            }
            return NotFound();
        }
        [HttpDelete("DeleteTask")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<IActionResult> DeleteTask([FromQuery] int taskId)
        {
            int rowChange = await taskService.RemoveTask(taskId);
            if (rowChange > 0)
            {
                return Ok("Xóa thành công");
            }
            return NotFound();
        }
        [HttpGet("Pagination")]
        [Authorize(Roles = "Admin,LineLeader")]
        public async Task<IActionResult> Pagination([FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            var result = await taskService.Pagination(pageSize, pageNumber);
            return Ok(result);
        }
        [HttpGet("GetTaskByUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTasksByUser([FromQuery] string id)
        {
            var user = await taskService.GetUserById(id);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng này");
            }
            var result = await taskService.GetTaskByUserAsync(user);
            return Ok(result);
        }
        [HttpGet("GetTaskById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTaskById([FromQuery] int IdTask)
        {
            var result = await taskService.GetTaskById(IdTask);
            if (result == null)
            {
                return NotFound("Không tìm thấy!");
            }
            return Ok(result);

        }
    }
}