namespace MoneyTracker.Models
{
    public class Transaction
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public DateTime Date { get; private set; } = DateTime.Now;
        public decimal Value { get; private set; }
        public string CategoryId { get; private set; }
        public Category TransactionCategory { get; private set; }
        public bool IsIncome { get; private set; }
    }
}
