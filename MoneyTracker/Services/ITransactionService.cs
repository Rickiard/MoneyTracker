namespace MoneyTracker.Services
{
    using MoneyTracker.Models;
    using MoneyTracker.DTOs;
    using MoneyTracker.Controllers;

    public interface ITransactionService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Transaction> GetTransactionByIdAsync(int id, int userId);
        Task<List<Transaction>> GetDashboardTransactionsAsync(int userId);
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(List<Transaction> transactions, int userId);
        Task AddTransactionAsync(Transaction transaction, int userId);
        Task EditTransactionAsync(Transaction transaction, int userId);
        Task DeleteTransactionAsync(int transactionId, int userId);
    }
}
