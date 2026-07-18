Here's the improved `README.md` file, incorporating the new content while maintaining the existing structure and information:

# CleanArchitecture.TaskManagement

A clean-architecture sample for task management built with .NET 8. This project demonstrates a layered architecture, EF Core persistence, simple authentication, and minimal API controllers for managing users and tasks.

## Features

- Clean Architecture (Domain, Application, Infrastructure, API)
- EF Core (SQL Server) persistence
- User registration and login (password hashing)
- Task CRUD operations with domain invariants
- Demo data seeding for development

## Requirements

- .NET 8 SDK
- SQL Server (or a compatible connection string)

## Quick Start

1. Clone the repository:

   ```bash
   git clone https://github.com/RenatoGynBr/CleanArchitecture.TaskManagement.git
   cd CleanArchitecture.TaskManagement
   ```

2. Configure the database connection string in `appsettings.Development.json` or set it via environment variables. Use the connection string name `TaskManagementDatabase`.

3. Run EF Core migrations (if any) or let the app create the database:

   ```bash
   dotnet ef database update --project ./CleanArchitecture.TaskManagement.Infrastructure --startup-project ./CleanArchitecture.TaskManagement.Api
   ```

4. Run the API:

   ```bash
   cd CleanArchitecture.TaskManagement.Api
   dotnet run
   ```

5. On the first run (in development), the app will automatically seed demo data when the database is empty.

## Demo Credentials (Development Only)

- Email: `demo@example.com`
- Password: `P@ssw0rd!`

**Important:** Do NOT use these credentials in production. Configure and override seed data via environment variables or by editing the seeder.

## API Endpoints (Summary)

- **POST** `/api/auth/register` — Register a new user
- **POST** `/api/auth/login` — Login and receive a token (placeholder until token service is implemented)
- **POST** `/api/tasks` — Create a task
- **GET** `/api/tasks/{id}` — Get task by id
- **PATCH** `/api/tasks/{id}/complete` — Complete a task
- **DELETE** `/api/tasks/{id}` — Delete a task

Refer to the controller code for request and response shapes. You can also use Swagger UI in Development mode for interactive API documentation.

## Seeding

The seeder runs at startup and will create a demo user and tasks only when the database is empty. To customize the seeding process, edit `Infrastructure/Data/DataSeeder.cs` or control execution via configuration settings.

## UI — Angular Application

The frontend Angular application is located in the `UI` folder. Below are the steps to document local development and build processes.

### Prerequisites

- Node.js 18+ and npm (or pnpm)
- Angular CLI (optional, `npx` works without a global install)

### Quick Start (Development)

1. Install dependencies and run the development server:

   ```bash
   cd UI
   npm install
   npx ng serve --proxy-config proxy.conf.json
   ```

2. The `proxy.conf.json` file (create it inside `UI`) should forward API requests to the running backend. Example:

   ```json
   {
     "/api": {
       "target": "https://localhost:5001",
       "secure": false,
       "changeOrigin": true,
       "logLevel": "debug"
     }
   }
   ```

3. Run the API (from the `CleanArchitecture.TaskManagement.Api` project) before starting the Angular dev server so the proxy can forward requests and the seeder will populate demo data.

### CORS (Alternative to Proxy)

If you prefer not to use the development proxy, enable CORS in the API during development. Example (add to `Program.cs`):

```csharp
builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// later, in the middleware pipeline
app.UseCors();
```

### Production Build and Hosting

To create a production build, run:

```bash
cd UI
npm run build -- --configuration production
```

- The output is placed in `dist/` (e.g., `dist/UI`). You can serve those static files from a CDN or copy them into the API project's `wwwroot` and enable `app.UseStaticFiles()`.
- When hosting under a sub-path, set the Angular `baseHref` at build time.

### Notes

- The Angular dev server proxy forwards `/api/*` to the backend; update `target` to match the port returned by `dotnet run` (HTTPS).
- For development, the README demo credentials may work (`demo@example.com` / `P@ssw0rd!`) if seeding is enabled and the database is empty.
- Never commit production credentials or secrets; use environment variables or secure configuration for tokens and keys.