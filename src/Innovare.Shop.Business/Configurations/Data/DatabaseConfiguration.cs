using Innovare.Shop.Business.Services.Common.Database;
using Innovare.Shop.Data.Configurations.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovare.Shop.Business.Configurations.Data
{
    public static class DatabaseConfiguration
    {
        public static void AddDataConfigurations(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddShopDbContext();
        }

        public static void UseDatabases(this IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var databaseService = serviceScope.ServiceProvider.GetService<IDatabaseService>();

            databaseService.ArrangeDatabasesAsync().Wait();
        }
    }
}