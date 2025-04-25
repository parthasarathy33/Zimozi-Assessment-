using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<Task?> GetTaskAsync(int id);
        Task<List<Task>> GetAllTasksAsync();
        Task<List<Task>> GetTasksByUserAsync(int userId);
        Task<Task> CreateTaskAsync(Task task);
        Task<Task?> UpdateTaskAsync(int id, Task task);
        Task<bool> DeleteTaskAsync(int id);
    }

    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Task?> GetTaskAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Task>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .ToListAsync();
        }

        public async Task<List<Task>> GetTasksByUserAsync(int userId)
        {
            return await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();
        }

        public async Task<Task> CreateTaskAsync(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Task?> UpdateTaskAsync(int id, Task task)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
                return null;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.DueDate = task.DueDate;
            existingTask.AssignedUserId = task.AssignedUserId;

            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 