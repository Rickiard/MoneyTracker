using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyTracker.Models;
using MoneyTracker.Services;
using System.Security.Claims;

namespace MoneyTracker.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }

        public async Task<IActionResult> Dashboard(string? searchTerm, string? type, int? categoryId, string? sortBy = "date", bool isAscending = false)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Authentication");

            var transactions = await _transactionService.GetDashboardTransactionsAsync(userId.Value, searchTerm, type, categoryId, sortBy, isAscending);
            var summary = await _transactionService.GetDashboardSummaryAsync(transactions, userId.Value);

            ViewBag.Summary = summary;
            ViewBag.Categories = await _transactionService.GetCategoriesAsync();
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Type = type;
            ViewBag.CategoryId = categoryId;
            ViewBag.SortBy = sortBy;
            ViewBag.IsAscending = isAscending;
            ViewBag.HasFilters = !string.IsNullOrWhiteSpace(searchTerm) || !string.IsNullOrWhiteSpace(type) || categoryId.HasValue;

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
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Authentication");

            if (ModelState.IsValid)
            {
                await _transactionService.AddTransactionAsync(transaction, userId.Value);
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Categories = await _transactionService.GetCategoriesAsync();

            return View(transaction);
        }

        public async Task<IActionResult> EditTransaction(int id)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Authentication");

            Transaction transaction = await _transactionService.GetTransactionByIdAsync(id, userId.Value);

            ViewBag.Categories = await _transactionService.GetCategoriesAsync();

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransaction(Transaction transaction)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Authentication");

            if (ModelState.IsValid)
            {
                await _transactionService.EditTransactionAsync(transaction, userId.Value);

                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Categories = await _transactionService.GetCategoriesAsync();

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Authentication");

            await _transactionService.DeleteTransactionAsync(id, userId.Value);

            return RedirectToAction(nameof(Dashboard));
        }
    }
}
