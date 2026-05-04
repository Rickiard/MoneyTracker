namespace MoneyTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
