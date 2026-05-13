# Money Tracker - Project Structure

## Organized File Structure

### Controllers/
- **TransactionsController.cs** - Handles all transaction CRUD operations

### DTOs/
- **DashboardSummaryDto.cs** - Data Transfer Object for the dashboard summary

### Models/
- **Entities/** - Domain model classes
  - Transaction.cs - Transaction entity
  - User.cs - User entity
  - Category.cs - Category entity
- **ViewModels/** - View-specific models
  - ErrorViewModel.cs - Error display model

### Services/
- **ITransactionService.cs** - Interface defining transaction service operations
- **TransactionService.cs** - Implementation of business logic for transactions

### Views/
- **Transactions/** - All transaction-related views
  - Dashboard.cshtml - Displays a list of all transactions with summary and filtering options
  - AddTransaction.cshtml - Form for adding a new transaction, with validation and category selection
  - EditTransaction.cshtml - Form for editing an existing transaction, pre-filled with current values
- **Shared/** - Shared layouts and components
  - _Layout.cshtml - Main layout template, includes navigation and shared page structure
  - _Layout.cshtml.css - Styles specific to the main layout
  - _ValidationScriptsPartial.cshtml - Partial for client-side validation scripts
  - Error.cshtml - Error display page for unhandled exceptions
  - _ViewImports.cshtml - Imports common namespaces and tag helpers for all views
  - _ViewStart.cshtml - Configures view startup settings (e.g., layout)

### Data/
- **ApplicationDbContext.cs** - Entity Framework database context

### Database/
- **MoneyTracker.db** - SQLite database file (auto-generated, not included in source control)

### Migrations/
- **20260513100136_SqLiteDatabase** - Database migration for initial schema creation
- **ApplicationDbContextModelSnapshot.cs** - Current model snapshot

### wwwroot/
- **css/site.css** - Custom CSS styles
- **js/site.js** - Custom JavaScript
- **manifest.json** - PWA manifest file
- **sw.js** - Service Worker for PWA support
- **lib/** - Third-party libraries (Bootstrap, jQuery, etc.)
- **bootstrap-icons/** - Icon fonts

### Properties/
- **launchSettings.json** - Project launch profiles and environment settings

### Root Files
- **Program.cs** - Application startup and configuration
- **appsettings.json** - Application settings
- **appsettings.Development.json** - Development environment settings
- **MoneyTracker.csproj** - Project file
- **Dockerfile** - Docker build instructions
- **docker-compose.yml** - Multi-container orchestration
- **.dockerignore** - Docker ignore rules