namespace MoneyTracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; } 
        public decimal Value { get; set; }
        public int CategoryId { get; set; }
        public Category? TransactionCategory { get; set; }
        public bool IsIncome { get; set; }
    }
}
