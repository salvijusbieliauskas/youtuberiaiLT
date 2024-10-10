using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer.Seed;
using DataAccessLayer.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IYoutuberRepository, YoutuberRepository>();

            services.AddSingleton<DailyChannelStore>();
            services.AddHostedService<DailyChannelSelectionRepository>();

            services.AddScoped<DbSeeder>();

            return services;
        }
    }
}
