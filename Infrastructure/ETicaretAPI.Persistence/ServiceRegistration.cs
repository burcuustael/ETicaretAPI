
using Microsoft.EntityFrameworkCore;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;


namespace ETicaretAPI.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<ETicaretAPIDbContext>(options =>
            options.UseNpgsql("User ID=burcu;Password=myPassword08;Host=localhost;Port=5432;Database=mydatabase;Pooling=true;Connection Lifetime=0;"));
    }
} 