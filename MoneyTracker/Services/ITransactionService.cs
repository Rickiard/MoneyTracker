namespace MoneyTracker.Services
{
    using MoneyTracker.Models;
    using MoneyTracker.DTOs;

    public interface ITransactionService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Transaction> GetTransactionByIdAsync(int id, int userId);
        Task<List<Transaction>> GetDashboardTransactionsAsync(int userId, string? searchTerm = null, string? type = null, int? categoryId = null, string? sortBy = "date", bool isAscending = false);
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(List<Transaction> transactions, int userId);
        Task AddTransactionAsync(Transaction transaction, int userId);
        Task EditTransactionAsync(Transaction transaction, int userId);
        Task DeleteTransactionAsync(int transactionId, int userId);
    }
}
