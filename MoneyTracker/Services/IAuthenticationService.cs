namespace MoneyTracker.Services
{
    using MoneyTracker.Models;

    public interface IAuthenticationService
    {
        Task<User?> Login(User user);

        Task Logout(User user);

        Task<bool> Register(User user); 
    }
}
