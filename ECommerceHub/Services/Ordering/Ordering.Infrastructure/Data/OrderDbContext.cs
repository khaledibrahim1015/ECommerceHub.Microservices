using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public  class OrderDbContext :DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
        
    }

    public DbSet<Order> Orders { get; set; }

 

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken =default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "khaled"; //TODO: This will be replaced Identity Server
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = "khaled"; //TODO: This will be replaced Identity Server
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
