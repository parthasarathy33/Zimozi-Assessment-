using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class TaskComment
    {
        public int Id { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int TaskId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public Task Task { get; set; } = null!;
        public User User { get; set; } = null!;
    }
} 