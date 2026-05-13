using Microsoft.EntityFrameworkCore;
using MoneyTracker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString, o =>
    {
        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }));

builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
    options.InstanceName = "MoneyTracker_";
});

var app = builder.Build();

app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transactions}/{action=Dashboard}/{id?}");

app.Run();
