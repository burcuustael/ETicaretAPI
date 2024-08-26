using ETicaret.Domain.Entities;
using ETicaret.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Contexts;

public class ETicaretAPIDbContext : DbContext
{
    public ETicaretAPIDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }   
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var datas = ChangeTracker
            .Entries<BaseEntity>();
        foreach (var data in datas)
        {
            _= data.State switch
            {
                EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified => data.Entity.UpdateDate = DateTime.UtcNow
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}