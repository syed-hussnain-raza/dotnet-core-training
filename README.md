# .NET Core Training

## Week 4 - Assignment 4

Extends the Week 3 User Management API (EF Core + SQL Server) with authentication and authorization: ASP.NET Core Identity, JWT bearer tokens, and route-level protection.

## What's New

- **ASP.NET Core Identity** - `AppDbContext` now inherits `IdentityDbContext<IdentityUser>`, adding Identity's own tables (`AspNetUsers`, `AspNetRoles`, etc.) alongside the existing `Users` table. Login/credentials are kept separate from business/domain data.
- **JWT authentication** - login issues a signed JWT (HMAC-SHA256) containing the user's id, email, and role claims. Every subsequent request is authenticated via the `Authorization: Bearer {token}` header - no server-side session.
- **Authorize all routes except Login** - a global fallback authorization policy (`RequireAuthenticatedUser()`) protects every endpoint by default. `AuthController` is the sole `[AllowAnonymous]` exception, so no changes were needed to `UserController` at all.
- **Auth service layer** - `IAuthService` / `AuthService` wraps `UserManager<IdentityUser>` and JWT generation, keeping `AuthController` as thin as `UserController`.
- **JWT token service** - `IJwtTokenService` / `JwtTokenService` builds and signs tokens, reading signing parameters (`Key`, `Issuer`, `Audience`, `ExpiryMinutes`) from configuration.
- **BaseApiController** - a shared base controller overriding `Ok()`/`BadRequest()` so every action automatically returns the `ApiResponse<T>` envelope; success/failure is inferred purely from which method is called, with no manual wrapping needed at each call site.
- **Extension methods** - service registration split into `Extensions/IdentityServiceExtensions.cs`, `JwtServiceExtensions.cs`, and `SwaggerServiceExtensions.cs`, keeping `Program.cs` short and readable.
- **Swagger JWT support** - an "Authorize" button in the Swagger UI lets you paste a token and test protected endpoints directly from the docs page.

## Endpoints

### Auth (public)
- `POST api/v1/auth/register` - Register a new login account
- `POST api/v1/auth/login`    - Log in and receive a JWT

### Users (requires a valid JWT)
- `GET api/v1/users`          - Get all users
- `GET api/v1/users/{id}`     - Get user by id
- `POST api/v1/users`         - Add new user
- `PUT api/v1/users/{id}`     - Update user
- `DELETE api/v1/users/{id}`  - Delete user

All responses are wrapped as:
```json
{
  "success": true,
  "message": "User fetched successfully.",
  "data": { }
}
```

## Authentication Flow

1. `POST /api/v1/auth/register` with `{ "email": "...", "password": "..." }`
2. `POST /api/v1/auth/login` with the same credentials - response `data` contains the JWT
3. In Swagger, click **Authorize** (top right) and enter `Bearer {token}`
4. All `Users` endpoints now succeed; without a token they return `401 Unauthorized`

## appsettings.json

```json
"Jwt": {
  "Key": "<at least 32 characters>",
  "Issuer": "MyAssignmentApi",
  "Audience": "MyAssignmentApiUsers",
  "ExpiryMinutes": 60
}
```

## Project Structure

```
MyAssignment/
├── Constants/       # ApiRoutesConstants, ApiVersionsConstants, MessagesConstants, MembershipTypesConstants
├── Controllers/     # BaseApiController, UserController, AuthController
├── Data/            # AppDbContext, Configurations/UserConfiguration
├── Dtos/            # UserDto, LoginDto, RegisterDto
├── Extensions/      # IdentityServiceExtensions, JwtServiceExtensions, SwaggerServiceExtensions
├── Helper/          # ApiResponse<T>, ApiPayload<T>, MappingProfile, ValidateModelStateFilter
├── Models/          # User
├── Services/        # IUserService, UserService, IAuthService, AuthService, IJwtTokenService, JwtTokenService
└── Program.cs
```

## How to Run

1. Clone the repo
2. Update the `DefaultConnection` and `Jwt:Key` values in `appsettings.json` for your environment
3. Run `dotnet ef database update` to apply migrations
4. Open in Visual Studio and run the project
5. Browse to `/swagger`, register a user, log in, authorize with the returned token, and explore the endpoints