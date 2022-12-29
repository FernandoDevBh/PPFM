using Domain.Identity;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class PersistenceServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
       .AddEntityFrameworkNpgsql()
       .AddDbContext<ApplicationDbContext>(options =>
       {
           options.UseLazyLoadingProxies();
           options.UseNpgsql(configuration.GetConnectionString("DBConnection"));
       });

        services
       .AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
       .AddEntityFrameworkStores<ApplicationDbContext>();
        return services;
    }
}
