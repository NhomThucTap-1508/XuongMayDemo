using Microsoft.AspNetCore.Identity;

public interface IUserService
{
    Task<ApplicationUser> GetUserByEmail(string email);
    Task<IdentityResult> CreateUser(UserModel userModel);

}