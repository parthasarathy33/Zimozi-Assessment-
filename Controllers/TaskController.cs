using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id)
        {
            var task = await _taskService.GetTaskAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetUserTasks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tasks = await _taskService.GetUserTasksAsync(userId);
            return Ok(tasks);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Models.Task>> CreateTask(Models.Task task)
        {
            var createdTask = await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Models.Task>> UpdateTask(int id, Models.Task task)
        {
            if (id != task.Id)
                return BadRequest();

            var updatedTask = await _taskService.UpdateTaskAsync(task);
            if (updatedTask == null)
                return NotFound();

            return Ok(updatedTask);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{taskId}/comments")]
        public async Task<ActionResult<TaskComment>> AddComment(int taskId, [FromBody] string content)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var comment = new TaskComment
            {
                TaskId = taskId,
                UserId = userId,
                Content = content
            };

            var createdComment = await _taskService.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetTaskComments), new { taskId }, createdComment);
        }

        [HttpGet("{taskId}/comments")]
        public async Task<ActionResult<IEnumerable<TaskComment>>> GetTaskComments(int taskId)
        {
            var comments = await _taskService.GetTaskCommentsAsync(taskId);
            return Ok(comments);
        }
    }
} 