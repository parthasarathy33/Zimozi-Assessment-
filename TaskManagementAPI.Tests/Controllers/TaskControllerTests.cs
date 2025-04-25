using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.Tests.Helpers;
using Xunit;

namespace TaskManagementAPI.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _mockContext = new Mock<ApplicationDbContext>(options);
            _controller = new TaskController(_mockContext.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ValidTask_ReturnsCreatedResult()
        {
            // Arrange
            var taskDto = new TaskDTO
            {
                Title = "Test Task",
                Description = "Test Description",
                AssignedUserId = 1,
                DueDate = DateTime.UtcNow.AddDays(7)
            };

            var user = new User { Id = 1, Username = "testuser", Role = UserRole.User };

            var mockUserDbSet = MockDbSetHelper.CreateMockDbSet(new List<User> { user });
            var mockTaskDbSet = MockDbSetHelper.CreateMockDbSet(new List<Models.Task>());

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);
            _mockContext.Setup(c => c.Tasks).Returns(mockTaskDbSet.Object);
            _mockContext.Setup(c => c.Users.FindAsync(1)).ReturnsAsync(user);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateTask(taskDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedTask = Assert.IsType<Models.Task>(createdResult.Value);
            Assert.Equal(taskDto.Title, returnedTask.Title);
            Assert.Equal(taskDto.Description, returnedTask.Description);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_InvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var taskDto = new TaskDTO
            {
                Title = "Test Task",
                Description = "Test Description",
                AssignedUserId = 999, // Non-existent user
                DueDate = DateTime.UtcNow.AddDays(7)
            };

            var mockUserDbSet = MockDbSetHelper.CreateMockDbSet(new List<User>());
            var mockTaskDbSet = MockDbSetHelper.CreateMockDbSet(new List<Models.Task>());

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);
            _mockContext.Setup(c => c.Tasks).Returns(mockTaskDbSet.Object);
            _mockContext.Setup(c => c.Users.FindAsync(999)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.CreateTask(taskDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Assigned user does not exist", badRequestResult.Value);
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
            var result = await _controller.GetTask(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTask = Assert.IsType<Models.Task>(okResult.Value);
            Assert.Equal(taskId, returnedTask.Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTask_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var taskId = 999;
            var mockDbSet = MockDbSetHelper.CreateMockDbSet(new List<Models.Task>());
            _mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.GetTask(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_NonExistentUser_ReturnsBadRequest()
        {
            // Arrange
            var taskDto = new TaskDTO
            {
                Title = "Test Task",
                Description = "Test Description",
                AssignedUserId = 999,
                DueDate = DateTime.UtcNow.AddDays(7)
            };

            var mockUserDbSet = MockDbSetHelper.CreateMockDbSet(new List<User>());
            var mockTaskDbSet = MockDbSetHelper.CreateMockDbSet(new List<Models.Task>());

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);
            _mockContext.Setup(c => c.Tasks).Returns(mockTaskDbSet.Object);
            _mockContext.Setup(c => c.Users.FindAsync(999)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.CreateTask(taskDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Assigned user does not exist", badRequestResult.Value);
        }
    }
} 