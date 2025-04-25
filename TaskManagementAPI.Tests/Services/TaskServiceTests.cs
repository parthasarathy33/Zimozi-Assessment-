using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using TaskManagementAPI.Tests.Helpers;
using Xunit;

namespace TaskManagementAPI.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly TaskService _taskService;
        private readonly List<Models.Task> _tasks;

        public TaskServiceTests()
        {
            // Create DbContextOptions for in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Create mock context with the options
            _mockContext = new Mock<ApplicationDbContext>(options);
            
            _tasks = new List<Models.Task>
            {
                new Models.Task { Id = 1, Title = "Task 1", Description = "Description 1" },
                new Models.Task { Id = 2, Title = "Task 2", Description = "Description 2" }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(_tasks);
            _mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);
            _taskService = new TaskService(_mockContext.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTask_ValidId_ReturnsTask()
        {
            // Arrange
            var taskId = 1;
            var task = new Models.Task
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description",
                AssignedUserId = 1
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(new List<Models.Task> { task });
            _mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);

            // Act
            var result = await _taskService.GetTask(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal(task.Title, result.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTask_InvalidId_ReturnsNull()
        {
            // Arrange
            var taskId = 999;
            var mockDbSet = MockDbSetHelper.CreateMockDbSet(new List<Models.Task>());
            _mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);

            // Act
            var result = await _taskService.GetTask(taskId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ReturnsAllTasks()
        {
            // Arrange
            var mockDbSet = MockDbSetHelper.CreateMockDbSet(_tasks);
            _mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);

            // Act
            var result = await _taskService.GetAllTasks();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Task 1", result[0].Title);
            Assert.Equal("Task 2", result[1].Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            var mockDbSet = MockDbSetHelper.CreateMockDbSet(new List<Models.Task>());
            _mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);

            // Act
            var result = await _taskService.GetAllTasks();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
} 