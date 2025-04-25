using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Task>>> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            var task = await _taskService.GetTaskAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Task>>> GetTasksByUser(int userId)
        {
            var tasks = await _taskService.GetTasksByUserAsync(userId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<Task>> CreateTask(Task task)
        {
            var createdTask = await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Task task)
        {
            var updatedTask = await _taskService.UpdateTaskAsync(id, task);
            if (updatedTask == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 