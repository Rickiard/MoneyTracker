using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Models;
using MoneyTracker.Services;

namespace MoneyTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var transactions = await _transactionService.GetDashboardTransactionsAsync(1);
            var summary = await _transactionService.GetDashboardSummaryAsync(transactions, 1);

            ViewBag.Summary = summary;

            return View(transactions);
        }

        public async Task<IActionResult> AddTransaction()
        {
            ViewBag.Categories = await _transactionService.GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.AddTransactionAsync(transaction, 1);
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Categories = await _transactionService.GetCategoriesAsync();
            return View(transaction);
        }

        public async Task<IActionResult> EditTransaction(int id)
        {
            Transaction transaction = await _transactionService.GetTransactionByIdAsync(id, 1);

            ViewBag.Categories = await _transactionService.GetCategoriesAsync();
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.EditTransactionAsync(transaction, 1);
              
                return RedirectToAction(nameof(Dashboard));
            }
            ViewBag.Categories = await _transactionService.GetCategoriesAsync();
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _transactionService.DeleteTransactionAsync(id, 1);

            return RedirectToAction(nameof(Dashboard));
        }
    }
}
