using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TaskManagementAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
        public ICollection<TaskComment> Comments { get; set; }
    }
} 