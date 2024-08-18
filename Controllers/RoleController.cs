
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService roleService;
    public RoleController(IRoleService roleService)
    {
        this.roleService = roleService;
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("SetRoleUser")]
    public async Task<IActionResult> Post(SetRoleModel setRoleModel)
    {
        var result = await roleService.SetRoleUser(setRoleModel);
        if (!result)
        {
            return StatusCode(500, "Khong the set roll");
        }
        return Ok(new
        {
            message = "set role thanh cong"
        });
    }
}
