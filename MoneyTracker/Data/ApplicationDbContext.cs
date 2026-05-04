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

        // Seed categorias pré-definidas
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Alimentação", Icon = "🍔", ColorHex = "#FF6B6B" },
            new Category { Id = 2, Name = "Transporte", Icon = "🚗", ColorHex = "#4ECDC4" },
            new Category { Id = 3, Name = "Saúde", Icon = "🏥", ColorHex = "#95E1D3" },
            new Category { Id = 4, Name = "Educação", Icon = "📚", ColorHex = "#A8E6CF" },
            new Category { Id = 5, Name = "Diversão", Icon = "🎮", ColorHex = "#FFD3B6" },
            new Category { Id = 6, Name = "Utilidades", Icon = "💡", ColorHex = "#FFAAA5" },
            new Category { Id = 7, Name = "Roupas", Icon = "👕", ColorHex = "#FF8B94" },
            new Category { Id = 8, Name = "Habitação", Icon = "🏠", ColorHex = "#8E99F3" },
            new Category { Id = 9, Name = "Viagem", Icon = "✈️", ColorHex = "#B19CD9" },
            new Category { Id = 10, Name = "Salário", Icon = "💰", ColorHex = "#52C41A" },
            new Category { Id = 11, Name = "Bónus", Icon = "🎁", ColorHex = "#FAAD14" },
            new Category { Id = 12, Name = "Outro", Icon = "📌", ColorHex = "#D9D9D9" }
        );
    }
}
