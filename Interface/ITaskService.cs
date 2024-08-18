public interface ITaskService
{
    Task<IList<TaskModel>> GetTasks();
    Task<int> CreateTask(CreateTaskModel createTaskModel, string userName);
    Task<int> UpdateTask(int taskID, UpdateTaskModel updateTaskModel);
    Task<int> RemoveTask(int taskID);
}