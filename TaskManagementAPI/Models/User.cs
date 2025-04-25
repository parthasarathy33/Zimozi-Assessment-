using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User;

        // Navigation properties
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }

    public enum UserRole
    {
        User,
        Admin
    }
} 