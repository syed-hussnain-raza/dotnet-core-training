# .NET Core Training

## Week 2 - Assignment 2

Extends the Week 1 User Management API with a layered architecture: services, dependency injection, AutoMapper, generic API responses, API versioning, and Swagger documentation.

## What's New

- **Generic responses** - every endpoint returns a consistent `{ success, message, data }` shape via `ApiResponse<T>`
- **Dependency injection** - `IUserService` is injected into the controller instead of the controller managing data directly
- **Service layer** - business logic moved out of the controller into `UserService`
- **AutoMapper** - maps `UserDto` to the `User` domain model
- **DTOs** - `UserDto` (in its own `Dtos` folder) defines the API's input contract, separate from the `User` domain model, with validation via DataAnnotations
- **Swagger** - interactive API docs and testing UI
- **Versioned routes** - all endpoints are now under `api/v{version}/users`

## Endpoints

- `GET api/v1/users`         - Get all users
- `GET api/v1/users/{id}`    - Get user by id
- `POST api/v1/users`        - Add new user
- `PUT api/v1/users/{id}`    - Update user
- `DELETE api/v1/users/{id}` - Delete user

All responses are wrapped as:
```json
{
  "success": true,
  "message": "User fetched successfully.",
  "data": { }
}
```

## Project Structure

```
MyAssignment/
├── Constants/       # UserMessages, MembershipTypes
├── Controllers/     # UserController
├── Dtos/            # UserDto
├── Helper/          # ApiResponse<T>, MappingProfile
├── Models/          # User
├── Services/        # IUserService, UserService
└── Program.cs
```

## How to Run

1. Clone the repo
2. Open in Visual Studio
3. Run the project
4. Browse to `/swagger` to explore and test endpoints interactively, or test with Postman