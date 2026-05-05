# Money Tracker

A simple and organized web application for tracking personal finances, built with ASP.NET Core (.NET 8) and Razor Pages.

## Features

- Add, edit, and delete transactions
- Categorize transactions
- View all transactions in a dashboard
- User-friendly forms with validation
- Responsive layout using Bootstrap
- **PWA support**: Can be installed as a mobile app when accessed via HTTPS (see below)
- **Dockerized**: Run with Docker and Docker Compose

## Project Structure

- **Controllers/**: Handles transaction CRUD operations
- **Models/Entities/**: Domain models (Transaction, User, Category)
- **Models/ViewModels/**: View-specific models (e.g., ErrorViewModel)
- **Views/Transactions/**: Transaction-related pages (Dashboard, Add, Edit)
- **Views/Shared/**: Layout, error, and shared partials
- **Data/**: Entity Framework database context
- **Migrations/**: Database schema migrations
- **wwwroot/**: Static files (JS, CSS, icons, libraries)

See `PROJECT_STRUCTURE.md` for a detailed structure.

## Getting Started

### Local Development

1. **Clone the repository:**
   ```sh
   git clone https://github.com/Rickiard/MoneyTracker.git
   ```
2. **Navigate to the project directory:**
   ```sh
   cd MoneyTracker/MoneyTracker
   ```
3. **Restore dependencies:**
   ```sh
   dotnet restore
   ```
4. **Apply migrations and update the database:**
   ```sh
   dotnet ef database update
   ```
5. **Run the application:**
   ```sh
   dotnet run
   ```
6. **Open in browser:**
   Visit `https://localhost:5001` (or the URL shown in the console)

### Run with Docker Compose

1. **Build and start the containers:**
   ```sh
   docker-compose up --build
   ```
2. The API will be available at `http://localhost:8080` and SQL Server at `localhost:1433`.
3. The connection string is pre-configured for Docker Compose in `docker-compose.yml`.

> **Note:** The default SQL Server password is set in `docker-compose.yml`. Change it for production use.

## Progressive Web App (PWA)

Money Tracker supports PWA features. When accessed via HTTPS on a mobile device, you can install the app directly to your home screen, providing a native-like experience with offline support and fast startup.

## Requirements

- .NET 8 SDK
- Docker & Docker Compose
- SQL Server (or use the included Docker service)
