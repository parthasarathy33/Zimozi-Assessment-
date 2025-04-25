# Task Management API

A simple .NET Core Web API for managing tasks with JWT authentication.

## Features

- User authentication with JWT
- Task management (CRUD operations)
- Role-based authorization (Admin, User)
- SQL Server database
- Swagger UI for API documentation

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or Visual Studio Code

## Setup

1. Clone the repository
2. Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. Update JWT settings in `appsettings.json`:
   ```json
   {
     "Jwt": {
       "Key": "your-secret-key-here",
       "Issuer": "your-issuer",
       "Audience": "your-audience",
       "ExpiryInMinutes": 30
     }
   }
   ```

4. Run the following commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

## API Endpoints

### Authentication

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and get JWT token

### Tasks

- `GET /api/task` - Get all tasks
- `GET /api/task/{id}` - Get task by ID
- `GET /api/task/user/{userId}` - Get tasks by user
- `POST /api/task` - Create a new task
- `PUT /api/task/{id}` - Update a task
- `DELETE /api/task/{id}` - Delete a task

## Testing

1. Open Swagger UI at `https://localhost:7071/swagger`
2. Register a new user
3. Login to get JWT token
4. Click "Authorize" and enter the token
5. Test the API endpoints

## Default Users

- Admin:
  - Email: admin@example.com
  - Password: Admin123!

- User:
  - Email: user@example.com
  - Password: User123!

## Database Schema

### Users
- Id (PK)
- Username
- Email
- Password
- Role

### Tasks
- Id (PK)
- Title
- Description
- Status
- CreatedAt
- DueDate
- AssignedUserId (FK)

### TaskComments
- Id (PK)
- Comment
- CreatedAt
- TaskId (FK)
- UserId (FK)
