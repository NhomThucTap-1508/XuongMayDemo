public interface IRoleService
{
    Task<bool> SetRoleUser(SetRoleModel setRoleModel);
}