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
- **DTOs/**: Data Transfer Objects for cross-layer data transfer (e.g., DashboardSummaryDto.cs)
- **Models/Entities/**: Domain models (Transaction, User, Category)
- **Models/ViewModels/**: View-specific models (e.g., ErrorViewModel)
- **Services/**: Business logic layer containing service interfaces and implementations
- **Views/Transactions/**: Transaction-related pages (Dashboard, Add, Edit)
- **Views/Shared/**: Layout, error, and shared partials
- **Data/**: Entity Framework database context
- **Migrations/**: Database schema migrations
- **wwwroot/**: Static files (JS, CSS, icons, libraries)

See `PROJECT_STRUCTURE.md` for a detailed structure.

## Getting Started

### Run with Docker Compose

1. **Clone the repository:**
   ```sh
   git clone https://github.com/Rickiard/MoneyTracker.git
   ```
2. **Navigate to the project directory:**
   ```sh
   cd MoneyTracker
   ```
3. **Build and start the containers:**
   ```sh
   docker-compose up --build
   ```
4. The API will be available at `http://localhost:8080` and SQL Server at `localhost:1433` and Redis Cache at `localhost:6379`.
5. The connection string is pre-configured for Docker Compose in `docker-compose.yml`.

> **Note:** The default SQL Server password is set in `docker-compose.yml`. Change it for production use.

## Progressive Web App (PWA)

Money Tracker supports PWA features. When accessed via HTTPS on a mobile device, you can install the app directly to your home screen, providing a native-like experience with offline support and fast startup.

## Requirements

- .NET 8 SDK
- Docker & Docker Compose
- SQL Server (or use the included Docker service)
