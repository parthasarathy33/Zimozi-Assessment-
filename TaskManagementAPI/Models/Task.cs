using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        // Foreign keys
        public int AssignedUserId { get; set; }

        // Navigation properties
        public User AssignedUser { get; set; } = null!;
        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }
} 