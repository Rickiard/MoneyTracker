using Microsoft.EntityFrameworkCore;
using MoneyTracker.Models;
using MoneyTracker.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MoneyTracker.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        public TransactionService(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var cacheKey = "categories_all";

            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<List<Category>>(cached)!;

            var categories = await _context.Categories.ToListAsync();

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(categories),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
                });

            return categories;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id, int userId)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<List<Transaction>> GetDashboardTransactionsAsync(int userId)
        {
            var cacheKey = $"transactions_dashboard_{userId}";

            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<List<Transaction>>(cached)!;

            var transactions = await _context.Transactions
                .Include(t => t.TransactionCategory)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(transactions),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            return transactions;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(List<Transaction> transactions, int userId)
        {
            var cacheKey = $"dashboard_summary_{userId}";

            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<DashboardSummaryDto>(cached)!;

            var categories = await GetCategoriesAsync();

            var income = new List<decimal>(new decimal[categories.Count]);
            var expenses = new List<decimal>(new decimal[categories.Count]);

            foreach (var transaction in transactions)
            {
                var index = categories.FindIndex(c => c.Id == transaction.CategoryId);
                if (index == -1) continue;

                if (transaction.IsIncome)
                    income[index] += transaction.Value;
                else
                    expenses[index] += transaction.Value;
            }

            var result = new DashboardSummaryDto
            {
                Labels = categories,
                Incomes = income,
                Expenses = expenses
            };

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            return result;
        }

        public async Task AddTransactionAsync(Transaction transaction, int userId)
        {
            transaction.UserId = userId;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            await InvalidateCache(userId);
        }

        public async Task EditTransactionAsync(Transaction transaction, int userId)
        {
            var existing = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == transaction.Id && t.UserId == userId);

            if (existing == null) return;

            existing.Description = transaction.Description;
            existing.Date = transaction.Date;
            existing.Value = transaction.Value;
            existing.CategoryId = transaction.CategoryId;
            existing.IsIncome = transaction.IsIncome;

            await _context.SaveChangesAsync();

            await InvalidateCache(userId);
        }

        public async Task DeleteTransactionAsync(int transactionId, int userId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

            if (transaction == null) return;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            await InvalidateCache(userId);
        }

        private async Task InvalidateCache(int userId)
        {
            await _cache.RemoveAsync($"transactions_dashboard_{userId}");
            await _cache.RemoveAsync($"dashboard_summary_{userId}");
        }
    }
}