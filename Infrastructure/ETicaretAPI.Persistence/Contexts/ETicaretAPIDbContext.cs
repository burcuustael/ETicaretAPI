using ETicaret.Domain.Entities;
using ETicaret.Domain.Entities.Common;
using ETicaret.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using File = ETicaret.Domain.Entities.File;

namespace ETicaretAPI.Persistence.Contexts;

public class ETicaretAPIDbContext : IdentityDbContext<AppUser,AppRole,string>
{
    public ETicaretAPIDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }   
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<ProductImageFile> ProductImageFiles { get; set; }
    public DbSet<InvoiceFile>InvoiceFiles { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var datas = ChangeTracker
            .Entries<BaseEntity>();
        foreach (var data in datas)
        {
            _= data.State switch
            {
                EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified => data.Entity.UpdateDate = DateTime.UtcNow,
                _ =>DateTime.UtcNow
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}