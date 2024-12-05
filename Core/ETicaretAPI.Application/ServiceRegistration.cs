using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaretAPI.Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection collection)
    {
        collection.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
        
    }
}