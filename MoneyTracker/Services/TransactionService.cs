namespace MoneyTracker.Services
{
    using Microsoft.EntityFrameworkCore;
    using MoneyTracker.Models;
    using MoneyTracker.DTOs;
    using Microsoft.Extensions.Caching.Distributed;

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
            return await _context.Categories.ToListAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id, int userId)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<List<Transaction>> GetDashboardTransactionsAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Transactions).FirstOrDefaultAsync(u => u.Id == userId);
            var transactions = user.Transactions
                .OrderByDescending(t => t.Date)
                .ToList();

            return transactions;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(List<Transaction> transactions, int userId)
        {
            var ChartLabels = new List<Category>();
            var ChartDataIncome = new List<decimal>();
            var ChartDataExpenses = new List<decimal>();

            if (transactions.Any())
            {
                foreach (Transaction transaction in transactions)
                {
                    transaction.TransactionCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == transaction.CategoryId);
                }

                List<decimal> chartDataIncome = new List<decimal>();
                List<decimal> chartDataExpenses = new List<decimal>();

                ChartLabels = _context.Categories.ToList();
                foreach (Category category in _context.Categories)
                {
                    decimal valueIncome = 0;
                    decimal valueExpenses = 0;

                    foreach (Transaction transaction in transactions)
                    {
                        if (transaction.CategoryId == category.Id)
                        {
                            if (transaction.IsIncome)
                            {
                                valueIncome += transaction.Value;
                            }
                            else
                            {
                                valueExpenses += transaction.Value;
                            }
                        }
                    }
                    chartDataIncome.Add(valueIncome);
                    chartDataExpenses.Add(valueExpenses);
                }
                ChartDataIncome = chartDataIncome;
                ChartDataExpenses = chartDataExpenses;
            }

            return new DashboardSummaryDto
            {
                Labels = ChartLabels,
                Expenses = ChartDataExpenses,
                Incomes = ChartDataIncome
            };
        }

        public async Task AddTransactionAsync(Transaction transaction, int userId)
        {
            var user = await _context.Users.Include(u => u.Transactions).FirstOrDefaultAsync(u => u.Id == userId);
            transaction.UserId = user.Id;
            user.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task EditTransactionAsync(Transaction transaction, int userId)
        {
            var user = await _context.Users.Include(u => u.Transactions).FirstOrDefaultAsync(u => u.Id == userId);
            var toChangeTransaction = user.Transactions.FirstOrDefault(t => t.Id == transaction.Id);
            toChangeTransaction.Description = transaction.Description;
            toChangeTransaction.Date = transaction.Date;
            toChangeTransaction.Value = transaction.Value;
            toChangeTransaction.CategoryId = transaction.CategoryId;
            toChangeTransaction.IsIncome = transaction.IsIncome;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(int transactionId, int userId)
        {
            var user = await _context.Users.Include(u => u.Transactions).FirstOrDefaultAsync(u => u.Id == userId);
            var toRemoveTransaction = user.Transactions.FirstOrDefault(t => t.Id == transactionId);
            _context.Transactions.Remove(toRemoveTransaction);
            await _context.SaveChangesAsync();
        }
    }
}
