using System.Collections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class TaskService : ITaskService
{
    private readonly DBContextUser context;
    private readonly UserManager<ApplicationUser> userManager;

    public TaskService(DBContextUser context, UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }
    public async Task<IList<TaskModel>> GetTasks()
    {
        return await context.Task
            .Include(t => t.line)
            .Select(t => new TaskModel
            {
                TaskID = t.TaskID,
                TaskName = t.TaskName,
                Note = t.Note,
                LineName = t.line.LineName,
                CreateBy = t.CreateBy
            }).ToListAsync();
    }
    public async Task<TaskModel> GetTaskById(int id)
    {
        return await context.Task.Where(t => t.TaskID == id)
        .Select(t => new TaskModel
        {
            TaskID = t.TaskID,
            TaskName = t.TaskName,
            Note = t.Note,
            LineName = t.line.LineName,
            CreateBy = t.CreateBy
        }).FirstOrDefaultAsync();
    }
    public async Task<ApplicationUser> GetUserById(string id)
    {
        return await userManager.FindByIdAsync(id);
    }
    public async Task<IList<TaskModel>> GetTaskByUserAsync(ApplicationUser user)
    {
        return await context.Task
            .Include(t => t.line)
            .Where(t => t.CreateBy == user.UserName)
            .Select(t => new TaskModel
            {
                TaskID = t.TaskID,
                TaskName = t.TaskName,
                Note = t.Note,
                LineName = t.line.LineName,
                CreateBy = t.CreateBy
            }).ToListAsync();
    }
    public async Task<IList<TaskModel>> Pagination(int pageSize, int pageNumber)
    {
        var skip = (pageNumber - 1) * pageSize;
        return await context.Task.Skip(skip).Take(pageSize).Select(t => new TaskModel
        {
            TaskID = t.TaskID,
            TaskName = t.TaskName,
            Note = t.Note,
            LineName = t.line.LineName,
            CreateBy = t.CreateBy
        }).ToArrayAsync();
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