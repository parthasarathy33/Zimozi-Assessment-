using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<Models.Task> GetTaskAsync(int id);
        Task<IEnumerable<Models.Task>> GetAllTasksAsync();
        Task<IEnumerable<Models.Task>> GetUserTasksAsync(string userId);
        Task<Models.Task> CreateTaskAsync(Models.Task task);
        Task<Models.Task> UpdateTaskAsync(Models.Task task);
        Task<bool> DeleteTaskAsync(int id);
        Task<TaskComment> AddCommentAsync(TaskComment comment);
        Task<IEnumerable<TaskComment>> GetTaskCommentsAsync(int taskId);
    }

    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Task> GetTaskAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Task>> GetUserTasksAsync(string userId)
        {
            return await _context.Tasks
                .Include(t => t.AssignedTo)
                .Where(t => t.AssignedToId.ToString() == userId)
                .ToListAsync();
        }

        public async Task<Models.Task> CreateTaskAsync(Models.Task task)
        {
            task.CreatedAt = DateTime.UtcNow;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Models.Task> UpdateTaskAsync(Models.Task task)
        {
            var existingTask = await _context.Tasks.FindAsync(task.Id);
            if (existingTask == null)
                return null;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Status = task.Status;
            existingTask.AssignedToId = task.AssignedToId;
            existingTask.UpdatedAt = DateTime.UtcNow;

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

        public async Task<TaskComment> AddCommentAsync(TaskComment comment)
        {
            comment.CreatedAt = DateTime.UtcNow;
            _context.TaskComments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<TaskComment>> GetTaskCommentsAsync(int taskId)
        {
            return await _context.TaskComments
                .Include(tc => tc.User)
                .Where(tc => tc.TaskId == taskId)
                .OrderByDescending(tc => tc.CreatedAt)
                .ToListAsync();
        }
    }
} 