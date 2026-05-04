using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Models;

namespace MoneyTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly ApplicationDbContext _context;

        public TransactionsController(ILogger<TransactionsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = _context.Users.Include(u => u.Transactions).FirstOrDefault(u => u.Id == 1);
            var transactions = user.Transactions
                .OrderByDescending(t => t.Date)
                .ToList();

            if (!transactions.Any())
            {
                transactions = null;
            }
            else
            {
                foreach (Transaction transaction in transactions)
                {
                    transaction.TransactionCategory = _context.Categories.FirstOrDefault(c => c.Id == transaction.CategoryId);
                }
            }

            List<decimal> chartDataIncome = new List<decimal>();
            List<decimal> chartDataExpenses = new List<decimal>();

            ViewBag.ChartLabels = _context.Categories.ToList();
            foreach(Category category in _context.Categories)
            {
                decimal valueIncome = 0;
                decimal valueExpenses = 0;

                foreach (Transaction transaction in transactions)
                {
                    if (transaction.CategoryId == category.Id)
                    {
                        if(transaction.IsIncome)
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
            ViewBag.ChartDataIncome = chartDataIncome;
            ViewBag.ChartDataExpenses = chartDataExpenses;

            return View(transactions);
        }

        public IActionResult AddTransaction()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Include(u => u.Transactions).FirstOrDefault(u => u.Id == 1);
                transaction.UserId = user.Id;
                user.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard), "Transactions");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(transaction);
        }

        public async Task<IActionResult> EditTransaction(int? id)
        {
            Transaction transaction = await _context.Transactions
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == 1);

            ViewBag.Categories = _context.Categories.ToList();
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Include(u => u.Transactions).FirstOrDefault(u => u.Id == 1);
                var toChangeTransaction = user.Transactions.FirstOrDefault(t => t.Id == transaction.Id);
                toChangeTransaction.Description = transaction.Description;
                toChangeTransaction.Date = transaction.Date;
                toChangeTransaction.Value = transaction.Value;
                toChangeTransaction.CategoryId = transaction.CategoryId;
                toChangeTransaction.IsIncome = transaction.IsIncome;
                await _context.SaveChangesAsync();
              
                return RedirectToAction(nameof(Dashboard), "Transactions");
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }
    }
}
