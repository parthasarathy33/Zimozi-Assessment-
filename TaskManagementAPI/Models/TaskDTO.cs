using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class TaskDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int AssignedUserId { get; set; }

        public DateTime? DueDate { get; set; }
    }
} 