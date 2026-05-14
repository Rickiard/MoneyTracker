using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MoneyTracker.Models;
using System.Text.Json;

namespace MoneyTracker.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthenticationService(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<bool> Register(User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.username == user.username);

            if (existingUser != null)
                return false;

            user.passwordHash = _passwordHasher.HashPassword(user, user.passwordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User?> Login(User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.username == user.username);

            if (existingUser == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(
                existingUser,
                existingUser.passwordHash,
                user.passwordHash
            );

            if (result == PasswordVerificationResult.Failed)
                return null;

            var cacheKey = $"user_session_{existingUser.Id}";
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
            };

            var userData = JsonSerializer.Serialize(existingUser);

            await _cache.SetStringAsync(cacheKey, userData, cacheOptions);

            return existingUser;
        }

        public async Task Logout(User user)
        {
            var cacheKey = $"user_session_{user.Id}";
            await _cache.RemoveAsync(cacheKey);
        }
    }
}