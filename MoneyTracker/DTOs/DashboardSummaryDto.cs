using MoneyTracker.Models;

namespace MoneyTracker.DTOs
{
    public class DashboardSummaryDto
    {
        public List<decimal> Incomes { get; set; }
        public List<decimal> Expenses { get; set; }
        public List<Category> Labels { get; set; }
    }
}
