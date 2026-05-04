# Money Tracker - Project Structure

## Organized File Structure

### 📁 Controllers/
- **TransactionsController.cs** - Handles all transaction CRUD operations

### 📁 Models/
- **Entities/** - Domain model classes
  - Transaction.cs - Transaction entity
  - User.cs - User entity
  - Category.cs - Category entity
- **ViewModels/** - View-specific models
  - ErrorViewModel.cs - Error display model

### 📁 Views/
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

### 📁 Data/
- **ApplicationDbContext.cs** - Entity Framework database context

### 📁 Migrations/
- **20260503231031_InitialDatabase.cs** - Initial database schema
- **20260503231031_InitialDatabase.Designer.cs** - Migration designer metadata
- **20260504094024_AddCategories.cs** - Add categories migration
- **20260504094024_AddCategories.Designer.cs**
- **20260504132153_UpdateCategoryNames.cs**
- **20260504132153_UpdateCategoryNames.Designer.cs**
- **20260504133152_DefaultUser.cs**
- **20260504133152_DefaultUser.Designer.cs**
- **20260504142053_TransactionChanged.cs**
- **20260504142053_TransactionChanged.Designer.cs**
- **20260504142705_TransactionChanged2.cs**
- **20260504142705_TransactionChanged2.Designer.cs**
- **20260504144850_CategoryIdToInt.cs**
- **20260504144850_CategoryIdToInt.Designer.cs**
- **ApplicationDbContextModelSnapshot.cs** - Current model snapshot

### 📁 wwwroot/
- **js/site.js** - Custom JavaScript
- **lib/** - Third-party libraries (Bootstrap, jQuery, etc.)
- **bootstrap-icons/** - Icon fonts

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
