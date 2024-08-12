using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ordering.Infrastructure.Configuration;

namespace Ordering.Infrastructure.Data;

public class OrderDbContextOptionFactory
{
    public AppSettings AppSettings { get; set; }
    public string ConnectionString => AppSettings.ConnectionString;
    public OrderDbContextOptionFactory(IOptions<AppSettings> options)
    {
        AppSettings = options.Value;
    }

    public DbContextOptions<OrderDbContext> CreateOrderDbContextOptions()
    {
        DbContextOptionsBuilder<OrderDbContext> optionBuilder = new DbContextOptionsBuilder<OrderDbContext>();
        optionBuilder.UseSqlServer(ConnectionString);
        return optionBuilder.Options;

    }


}
