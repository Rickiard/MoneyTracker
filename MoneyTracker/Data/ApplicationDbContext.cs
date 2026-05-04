using Microsoft.EntityFrameworkCore;
using MoneyTracker.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Value)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Food & Dining", Icon = "🍔", ColorHex = "#FF6B6B" },
            new Category { Id = 2, Name = "Transportation", Icon = "🚗", ColorHex = "#4ECDC4" },
            new Category { Id = 3, Name = "Health", Icon = "🏥", ColorHex = "#95E1D3" },
            new Category { Id = 4, Name = "Education", Icon = "📚", ColorHex = "#A8E6CF" },
            new Category { Id = 5, Name = "Entertainment", Icon = "🎮", ColorHex = "#FFD3B6" },
            new Category { Id = 6, Name = "Utilities", Icon = "💡", ColorHex = "#FFAAA5" },
            new Category { Id = 7, Name = "Clothing", Icon = "👕", ColorHex = "#FF8B94" },
            new Category { Id = 8, Name = "Housing", Icon = "🏠", ColorHex = "#8E99F3" },
            new Category { Id = 9, Name = "Travel", Icon = "✈️", ColorHex = "#B19CD9" },
            new Category { Id = 10, Name = "Salary", Icon = "💰", ColorHex = "#52C41A" },
            new Category { Id = 11, Name = "Bonus", Icon = "🎁", ColorHex = "#FAAD14" },
            new Category { Id = 12, Name = "Other", Icon = "📌", ColorHex = "#D9D9D9" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1 }
        );
    }
}
