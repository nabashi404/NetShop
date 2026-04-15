using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetShop.Data;

namespace NetShop;

public static class DatabaseExtensions
{
    public static IServiceCollection AddAppDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(config.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddAppIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
