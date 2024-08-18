using Microsoft.AspNetCore.Identity;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> userManager;
    public readonly DBContextUser context;
    public UserService(DBContextUser context, UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }
    public async Task<ApplicationUser> GetUserByEmail(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }
    public async Task<IdentityResult> CreateUser(UserModel userModel)
    {
        var newUser = new ApplicationUser
        {
            UserName = userModel.UserName,
            Email = userModel.Email,
            PhoneNumber = userModel.PhoneNumber
        };

        return await userManager.CreateAsync(newUser, userModel.Password);
    }

}