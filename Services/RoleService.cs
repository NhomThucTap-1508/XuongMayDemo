
using Microsoft.AspNetCore.Identity;

public class RoleService : IRoleService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    public RoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<bool> SetRoleUser(SetRoleModel setRoleModel)
    {
        var user = await userManager.FindByIdAsync(setRoleModel.UserID);
        if (user == null)
        {
            return false;
        }
        var roleExists = await roleManager.RoleExistsAsync(setRoleModel.RoleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(setRoleModel.RoleName));
        }
        var addToRole = await userManager.AddToRoleAsync(user, setRoleModel.RoleName);
        if (addToRole.Succeeded)
        {
            return true;
        }
        return false;

    }
}