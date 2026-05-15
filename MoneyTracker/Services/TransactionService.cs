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

        public async Task<List<Transaction>> GetDashboardTransactionsAsync(
            int userId,
            string? searchTerm = null,
            string? type = null,
            int? categoryId = null,
            string? sortBy = "date",
            bool isAscending = false)
        {
            var versionKey = $"dashboard_cache_version_{userId}";
            var version = await _cache.GetStringAsync(versionKey) ?? "1";

            var cacheKey = $"transactions_dashboard_{userId}_v{version}_{searchTerm}_{type}_{categoryId}_{sortBy}_{isAscending}";

            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<List<Transaction>>(cached)!;

            var query = _context.Transactions
                .Include(t => t.TransactionCategory)
                .Where(t => t.UserId == userId);

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t => t.Description.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                if (type.Equals("income", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(t => t.IsIncome);
                else if (type.Equals("expense", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(t => !t.IsIncome);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            // SQLite-safe sorting
            List<Transaction> transactions;

            if (sortBy?.ToLower() == "amount")
            {
                transactions = await query.ToListAsync();

                transactions = isAscending
                    ? transactions.OrderBy(t => t.IsIncome ? t.Value : -t.Value).ToList()
                    : transactions.OrderByDescending(t => t.IsIncome ? t.Value : -t.Value).ToList();
            }
            else
            {
                query = sortBy?.ToLower() switch
                {
                    "description" => isAscending
                        ? query.OrderBy(t => t.Description)
                        : query.OrderByDescending(t => t.Description),

                    _ => isAscending
                        ? query.OrderBy(t => t.Date)
                        : query.OrderByDescending(t => t.Date)
                };

                transactions = await query.ToListAsync();
            }

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
            var versionKey = $"dashboard_cache_version_{userId}";
            var version = await _cache.GetStringAsync(versionKey) ?? "1";

            var transactionHash = string.Join("_",
                transactions
                    .OrderBy(t => t.Id)
                    .Select(t => $"{t.Id}-{t.Value}-{t.CategoryId}-{t.IsIncome}")
            );

            var cacheKey = $"dashboard_summary_{userId}_v{version}_{transactionHash.GetHashCode()}";

            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<DashboardSummaryDto>(cached)!;

            var categories = await GetCategoriesAsync();

            var labels = new List<Category>();
            var incomes = new List<decimal>();
            var expenses = new List<decimal>();

            foreach (var category in categories)
            {
                var categoryTransactions = transactions
                    .Where(t => t.CategoryId == category.Id)
                    .ToList();

                var totalIncome = categoryTransactions
                    .Where(t => t.IsIncome)
                    .Sum(t => t.Value);

                var totalExpense = categoryTransactions
                    .Where(t => !t.IsIncome)
                    .Sum(t => t.Value);

                if (totalIncome > 0 || totalExpense > 0)
                {
                    labels.Add(category);
                    incomes.Add(totalIncome);
                    expenses.Add(totalExpense);
                }
            }

            var result = new DashboardSummaryDto
            {
                Labels = labels,
                Incomes = incomes,
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
            var versionKey = $"dashboard_cache_version_{userId}";

            var currentVersion = await _cache.GetStringAsync(versionKey);

            int newVersion = string.IsNullOrEmpty(currentVersion)
                ? 2
                : int.Parse(currentVersion) + 1;

            await _cache.SetStringAsync(
                versionKey,
                newVersion.ToString(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
                });

            await _cache.RemoveAsync($"transactions_dashboard_{userId}");
        }
    }
}