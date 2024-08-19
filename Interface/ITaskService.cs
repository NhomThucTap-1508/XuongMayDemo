public interface ITaskService
{
    Task<IList<TaskModel>> GetTasks();
    Task<int> CreateTask(CreateTaskModel createTaskModel, string userName);
    Task<int> UpdateTask(int taskID, UpdateTaskModel updateTaskModel);
    Task<int> RemoveTask(int taskID);
    Task<TaskModel> GetTaskById(int id);
    Task<ApplicationUser> GetUserById(string id);
    Task<IList<TaskModel>> GetTaskByUserAsync(ApplicationUser user);
    Task<IList<TaskModel>> Pagination(int pageSize, int pageNumber);
}