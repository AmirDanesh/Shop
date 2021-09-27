using Innovare.Shop.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Innovare.Shop.Data.Configurations.DbContext
{
    public static class DbContextConfiguration
    {
        public static void AddShopDbContext(this IServiceCollection serviceCollection)
        {
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var configurationSection = serviceProvider.GetService<IConfiguration>().GetSection("databaseContext");
            var databaseContextOptions = configurationSection.Get<DbContextOptions>();

            if (databaseContextOptions is not null && databaseContextOptions.IsEnabled)
            {
                serviceCollection.AddDbContext<ShopDbContext>(options =>
                     options.UseSqlServer("Fake Connection String",
                           sqlServerOptionsAction =>
                           {
                               sqlServerOptionsAction.EnableRetryOnFailure
                               (
                                   maxRetryCount: 5,
                                   maxRetryDelay: TimeSpan.FromSeconds(2),
                                   errorNumbersToAdd: null
                               );
                           }), ServiceLifetime.Scoped);
            }
        }
    }
}