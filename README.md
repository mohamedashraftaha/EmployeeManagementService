# EmployeeManagementService

A fullstack employee management system with a .NET (C#) backend and Angular frontend. This README explains the architecture, implementation details, and how to run the project.

---

## Table of Contents
- [Project Overview](#project-overview)
- [Backend (ASP.NET Core)](#backend-aspnet-core)
  - [Architecture & Features](#architecture--features)
  - [Authentication & Security](#authentication--security)
  - [Employee Management](#employee-management)
  - [Sqllite Database & Seeding](#sqllite-database--seeding)
  - [How to Run the Backend](#how-to-run-the-backend)
- [Frontend (Angular)](#frontend-angular)
  - [Architecture & Features](#architecture--features-1)
  - [JWT Handling](#jwt-handling)
  - [How to Run the Frontend](#how-to-run-the-frontend)
- [Testing](#testing)
- [Notes & Improvements](#notes--improvements)

---

## Project Overview
This system allows users to register, log in, and view a list of employees. Authentication is handled via JWT. The backend is built with ASP.NET Core and uses an in-memory database for demo/testing. The frontend is built with Angular and demonstrates secure JWT-based authentication and protected routes.

---

## Backend (ASP.NET Core)

### Architecture & Features
- **Clean separation** of concerns: Controllers, Services, Repositories, Domain Models.
- **Controllers**: Handle HTTP requests for authentication (`AuthController`) and employee management (`EmployeeController`).
- **Services**: Business logic for authentication, JWT generation, and employee operations.
- **Repositories**: Data access for employees and users, using Entity Framework Core.
- **Unit of Work**: Ensures transactional integrity for database operations.

### Authentication & Security
- **Registration**: Stores the hashed password for each user. No raw passwords or hashes are stored.
- **Login**: Decrypts the stored password and compares it to the user input.
- **JWT**: On successful login, a JWT is issued and must be used for all protected API calls.
- **Error Handling**: All controllers and services use try-catch for robust error handling and logging.

### Employee Management
- **CRUD operations** for employees (the demo UI focuses on listing employees).
- **Seeded Data**: The in-memory database is seeded with sample employees for immediate testing.

### Sqllite Database & Seeding
- Uses Entity Framework Core's in-memory provider for easy testing and demo.
- Employees are seeded in `EmployeeDbContext.OnModelCreating`.

### How to Run the Backend
1. **Navigate to the backend directory:**
   ```bash
   cd EmployeeManagementService-Backend/EmployeeManagementService-Backend
   ```
2. **Restore dependencies and run:**
   ```bash
   dotnet restore
   dotnet run
   ```
3. **API will be available at:**
   - `https://localhost:7241/`
   - Swagger/OpenAPI UI (if in development mode): `https://localhost:7241/swagger`

---

## Frontend (Angular)

### Architecture & Features
- **Services**: `AuthService` for authentication (login/register, JWT storage), `EmployeeService` for employee API calls.
- **Components**:
  - `LoginComponent`: Login form, handles JWT and errors.
  - `RegisterComponent`: Registration form, handles errors and success.
  - `EmployeeListComponent`: Displays employees (protected route).
- **JWT Interceptor**: Attaches JWT to all outgoing HTTP requests.
- **AuthGuard**: Protects employee routes, redirects unauthenticated users to login.
- **Navigation**: Simple navbar for login, register, employees, and logout.

### JWT Handling
- JWT is stored in `localStorage` after login.
- All API requests include the JWT in the `Authorization` header.
- Logout removes the JWT and updates the UI.

### How to Run the Frontend
1. **Navigate to the frontend directory:**
   ```bash
   cd employee-management-frontend
   ```
2. **Install dependencies:**
   ```bash
   npm install
   ```
3. **Run the Angular development server:**
   ```bash
   ng serve --port 4200
   ```
4. **Open your browser at:**
   - `http://localhost:4200/`

---

## Testing
- **Backend**: You can use Swagger UI or Postman to test the API endpoints.
- **Frontend**: Use the UI to register, log in, and view employees. Try logging out and accessing protected routes to see the guard in action.

---

## Notes & Improvements
- **Security**: Passwords are hashed for demo purposes.
- **Persistence**: The sqllite database is for demo/testing only. For production, switch to a persistent database (e.g., SQL Server, PostgreSQL).
- **Validation**: Add more robust validation and error handling as needed.
- **Extensibility**: The architecture supports easy extension for more features (CRUD for employees, user roles, etc.).

---
