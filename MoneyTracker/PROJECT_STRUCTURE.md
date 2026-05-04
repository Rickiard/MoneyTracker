# Money Tracker - Project Structure

## Organized File Structure

### 📁 Controllers/
- **DashboardController.cs** - Main controller handling all transaction CRUD operations
  - Dashboard() - Display all transactions
  - AddTransaction() - Create new transaction
  - EditTransaction() - Update existing transaction
  - DeleteTransaction() - Remove transaction

### 📁 Models/
- **Entities/** - Domain model classes
  - Transaction.cs - Transaction entity
  - User.cs - User entity
  - Category.cs - Category entity
- **ViewModels/** - View-specific models
  - ErrorViewModel.cs - Error display model

### 📁 Views/
- **Transactions/** - All transaction-related views
  - Dashboard.cshtml - Main dashboard with transaction list
  - AddTransaction.cshtml - Add new transaction form
  - EditTransaction.cshtml - Edit existing transaction form
- **Shared/** - Shared layouts and components
  - _Layout.cshtml - Main layout template
  - _ValidationScriptsPartial.cshtml - Validation scripts
  - Error.cshtml - Error page
  - _ViewImports.cshtml - Global view imports
  - _ViewStart.cshtml - View startup configuration

### 📁 Data/
- **ApplicationDbContext.cs** - Entity Framework database context

### 📁 Migrations/
- **20260503231031_InitialDatabase.cs** - Initial database schema
- **20260503231031_InitialDatabase.Designer.cs** - Migration designer metadata
- **ApplicationDbContextModelSnapshot.cs** - Current model snapshot

### 📁 Root Files
- **Program.cs** - Application startup and configuration
- **appsettings.json** - Application settings
- **MoneyTracker.csproj** - Project file

## Key Improvements

✅ **Better Organization**: Models separated into Entities and ViewModels  
✅ **Unified Views**: All transaction views in one folder (Views/Transactions/)  
✅ **Removed Redundancy**: Deleted unused AddTransactionController  
✅ **Clear Separation of Concerns**: Each folder has a specific purpose  
✅ **Scalability**: Easy to add new features (e.g., Reports, Analytics)  

## Future Improvements (Optional)

- Add **Services/** folder for business logic (TransactionService, etc.)
- Add **Repositories/** folder for data access patterns
- Add **Extensions/** folder for helper extensions
- Create **wwwroot/css/** and **wwwroot/js/** for custom styles and scripts
- Add **Middleware/** folder if custom middleware is needed
