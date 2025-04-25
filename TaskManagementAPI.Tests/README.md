# Task Management API Tests

This project contains unit tests for the Task Management API using xUnit and Moq.

## Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2022 or Visual Studio Code
- Test Explorer (in Visual Studio) or .NET CLI

## Running Tests

### Using Visual Studio
1. Open the solution in Visual Studio
2. Open Test Explorer (Test > Test Explorer)
3. Click "Run All Tests" or select specific tests to run

### Using .NET CLI
```bash
# Navigate to the test project directory
cd TaskManagementAPI.Tests

# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run a specific test class
dotnet test --filter "FullyQualifiedName=TaskManagementAPI.Tests.Controllers.TaskControllerTests"

# Run a specific test method
dotnet test --filter "FullyQualifiedName=TaskManagementAPI.Tests.Controllers.TaskControllerTests.CreateTask_ValidTask_ReturnsCreatedResult"
```

## Test Categories

### Controller Tests
- `TaskControllerTests`: Tests for the Task API endpoints
  - CreateTask_ValidTask_ReturnsCreatedResult
  - CreateTask_InvalidUser_ReturnsBadRequest
  - GetTask_ValidId_ReturnsTask
  - GetTask_InvalidId_ReturnsNotFound

### Service Tests
- `TaskServiceTests`: Tests for the Task service layer
  - GetTask_ValidId_ReturnsTask
  - GetTask_InvalidId_ReturnsNull
  - GetAllTasks_ReturnsAllTasks
  - GetAllTasks_EmptyDatabase_ReturnsEmptyList

## Test Structure

Each test follows the Arrange-Act-Assert pattern:
1. **Arrange**: Set up test data and dependencies
2. **Act**: Execute the code being tested
3. **Assert**: Verify the results

## Mocking

The tests use Moq to mock:
- Database context
- Entity Framework DbSet
- Dependencies

## Code Coverage

To generate code coverage reports:

```bash
# Install coverlet.collector
dotnet add package coverlet.collector

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"TestResults/*/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

## Best Practices

1. Each test should be independent
2. Use meaningful test names
3. Follow the single responsibility principle
4. Mock external dependencies
5. Test both success and failure scenarios
6. Keep tests simple and focused 