using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ordering.Infrastructure.Configuration;

namespace Ordering.Infrastructure.Data;

public class OrderDbContextOptionFactory
{
    public AppSettings AppSettings { get; set; }
    public OrderDbContextOptionFactory(IOptions<AppSettings> options)
    {
        AppSettings = options.Value;
    }

    public DbContextOptions<OrderDbContext> CreateOrderDbContextOptions()
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? AppSettings.ConnectionString;
        DbContextOptionsBuilder<OrderDbContext> optionBuilder = new DbContextOptionsBuilder<OrderDbContext>();
        optionBuilder.UseSqlServer(connectionString);
        return optionBuilder.Options;

    }


}
