using Microsoft.EntityFrameworkCore;

public class TaskService : ITaskService
{
    private readonly DBContextUser context;

    public TaskService(DBContextUser context)
    {
        this.context = context;
    }
    public async Task<IList<TaskModel>> GetTasks()
    {
        return await context.Task.Join(context.Line,
                task => task.LineID,
                line => line.LineID, (task, line) => new TaskModel
                {
                    TaskID = task.TaskID,
                    TaskName = task.TaskName,
                    Note = task.Note,
                    LineName = line.LineName,
                    CreateBy = task.CreateBy,
                })
                .ToListAsync();
    }
    public async Task<int> CreateTask(CreateTaskModel createTaskModel, string userName)
    {
        Task task = new Task
        {
            LineID = createTaskModel.LineID,
            OrderID = createTaskModel.OrderID,
            TaskName = createTaskModel.TaskName,
            Note = createTaskModel.Note,
            CreateBy = userName

        };
        await context.Task.AddAsync(task);
        int rowChange = await context.SaveChangesAsync();
        return rowChange;
    }
    public async Task<int> UpdateTask(int taskID, UpdateTaskModel updateTaskModel)
    {
        var task = await context.Task.FindAsync(taskID);
        if (task == null)
        {
            return 0;
        }
        task.TaskName = updateTaskModel.TaskName;
        task.Note = updateTaskModel.Note;
        context.Task.Update(task);
        int rowChange = await context.SaveChangesAsync();
        return rowChange;
    }
    public async Task<int> RemoveTask(int taskID)
    {
        var task = await context.Task.FindAsync(taskID);
        if (task == null)
        {
            return 0;
        }
        context.Task.Remove(task);
        var rowChange = await context.SaveChangesAsync();
        return rowChange;
    }
}