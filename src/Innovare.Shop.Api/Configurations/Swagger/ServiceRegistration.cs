using Innovare.Shop.Api.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;

namespace Innovare.Shop.Api.Configurations.Swagger
{
    public static class ServiceRegistration
    {
        public static void AddSwagger(this IServiceCollection serviceCollection)
        {
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var configuration = serviceProvider.GetService<IConfiguration>();
            var configurationSection = configuration.GetSection("swagger");
            var swaggerOptions = configurationSection.Get<SwaggerOptions>();

            if (swaggerOptions is null)
            {
                throw new InvalidOperationException(Messages.SwaggerSectionNotFound);
            }

            if (swaggerOptions.IsEnabled)
            {
                serviceCollection.Configure<SwaggerOptions>(configurationSection);

                serviceCollection.AddSwaggerDocument(options =>
                {
                    options.Title = swaggerOptions.Title;
                    options.Version = swaggerOptions.Version;
                    options.Description = swaggerOptions.Description;

                    if (swaggerOptions.Security is not null && swaggerOptions.Security.IsEnabled)
                    {
                        options.AddSecurity(swaggerOptions.Security.Scheme, new OpenApiSecurityScheme()
                        {
                            Type = swaggerOptions.Security.Type,
                            Scheme = swaggerOptions.Security.Scheme,
                            Name = swaggerOptions.Security.Name,
                            In = swaggerOptions.Security.In,
                            Description = swaggerOptions.Security.Description
                        });

                        options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor(swaggerOptions.Security.Scheme));
                    }
                });
            }
        }

        public static void UseSwagger(this IApplicationBuilder applicationBuilder)
        {
            var swaggerOptions = applicationBuilder.ApplicationServices.GetService<IConfiguration>().GetSection("swagger").Get<SwaggerOptions>();

            if (swaggerOptions is not null &&
                swaggerOptions.IsEnabled)
            {
                applicationBuilder.UseStaticFiles();
                applicationBuilder.UseOpenApi();
                applicationBuilder.UseSwaggerUi3(options =>
                {
                    options.Path = swaggerOptions.UiPath;
                });
            }
        }
    }
}