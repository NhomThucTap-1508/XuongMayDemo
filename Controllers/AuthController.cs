
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IAuthService authService;
    public AuthController(IUserService userService, IAuthService authService)
    {
        this.userService = userService;
        this.authService = authService;
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var result = await authService.CheckUser(loginModel.UserName);
        if (result == null)
        {
            return Unauthorized("Không tìm thấy người dùng");
        }
        var resultPassword = await authService.CheckPassword(loginModel);
        if (!resultPassword.Succeeded)
        {
            return Unauthorized("Không đúng mật khẩu");
        }
        var token = authService.GenerateJwtToken(result);
        return Ok(new
        {
            access_token = token,
            token_type = "JWT",
            auth_type = "Bearer",
            expires_in = DateTime.UtcNow.AddHours(1),
            user = new
            {
                userName = result.UserName,
                email = result.Email
            }
        });

    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserModel userModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingUser = await userService.GetUserByEmail(userModel.Email);
        if (existingUser != null)
        {
            return Conflict("User đã tồn tại!");
        }
        var result = await userService.CreateUser(userModel);
        if (result.Succeeded)
        {
            return Ok("Tạo thành công");
        }
        return StatusCode(500, "Lỗi khi tạo người dùng");
    }
}