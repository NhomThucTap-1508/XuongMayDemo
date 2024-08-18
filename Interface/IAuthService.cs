using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<ApplicationUser> CheckUser(string userName);
    Task<SignInResult> CheckPassword(LoginModel loginModel);
    string GenerateJwtToken(ApplicationUser user);
}