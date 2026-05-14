using System.ComponentModel.DataAnnotations;

namespace MoneyTracker.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string passwordHash { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
