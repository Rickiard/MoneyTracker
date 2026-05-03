namespace MoneyTracker.Models
{
    public class User
    {
        private int Id { get; set; }
        private string Username { get; set; }
        private string Email { get; set; }
        private string PasswordHash { get; set; }
        private DateTime CreatedAt { get; set; }
    }
}
