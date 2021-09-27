using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Innovare.Shop.Business.Configurations.Mapper
{
    public static class MapperConfiguration
    {
        public static void AddMappers(this IServiceCollection serviceCollection)
        {
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var configurationSection = serviceProvider.GetService<IConfiguration>().GetSection("mapper");

            serviceCollection.Configure<MapperOptions>(configurationSection);

            serviceCollection.AddMapper<MapperOptions>();
        }
    }
}