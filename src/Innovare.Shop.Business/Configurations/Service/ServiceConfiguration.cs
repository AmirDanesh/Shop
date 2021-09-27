using Innovare.Shop.Business.Abstractions.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovare.Shop.Business.Configurations.Service
{
    public static class ServiceConfiguration
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var configurationSection = serviceProvider.GetService<IConfiguration>().GetSection("service");

            serviceCollection.Configure<ServiceOptions>(configurationSection);

            typeof(ServiceBase)
                .Assembly
                .DefinedTypes
                .Where(typeInfo
                    => !typeInfo.IsAbstract
                    && typeInfo.IsSubclassOf(typeof(ServiceBase)))
                .ToList()
                .ForEach(typeInfo =>
                {
                    var internalInterface = typeInfo.GetInterface($"I{typeInfo.Name}");
                    var publicInterface = typeInfo.GetInterface($"I{typeInfo.Name.Replace("Service", "OperationService")}");
                    var validationInterface = typeInfo.GetInterface($"I{typeInfo.Name.Replace("Service", "ValidationService")}");

                    serviceCollection.AddScoped(internalInterface, typeInfo);

                    if (publicInterface is not null)
                    {
                        serviceCollection.AddScoped(publicInterface, implementationFactory => implementationFactory.GetService(internalInterface));
                    }

                    if (validationInterface is not null)
                    {
                        serviceCollection.AddScoped(validationInterface, implementationFactory => implementationFactory.GetService(internalInterface));
                    }
                });

            //serviceCollection.AddScoped<ITenantManagerService, TenantManagerService>();
            //serviceCollection.AddScoped(typeof(ITenantManagerOperationService), implementationFactory => implementationFactory.GetService<ITenantManagerService>());
            //serviceCollection.AddScoped(typeof(ITenantManagerValidationService), implementationFactory => implementationFactory.GetService<ITenantManagerService>());
        }
    }
}