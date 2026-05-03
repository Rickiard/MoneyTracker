namespace MoneyTracker.Models
{
    public class Transaction
    {
        private int Id { get; set; }
        private int UserId { get; set; }
        private User User { get; set; }
        private string Description { get; set; } = string.Empty;
        private DateTime Date { get; set; } = DateTime.Now;
        private decimal Value { get; set; }
        private string CategoryId { get; set; }
        private Category TransactionCategory { get; set; }
        private bool IsIncome { get; set; }
    }
}
