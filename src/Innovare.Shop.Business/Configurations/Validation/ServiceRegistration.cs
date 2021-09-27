using FluentValidation;
using FluentValidation.AspNetCore;
using Innovare.Shop.Business.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Innovare.Shop.Business.Configurations.Validation
{
    public static class ServiceRegistration
    {
        public static void AddValidation(this IServiceCollection serviceCollection)
        {
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var configurationSection = configuration.GetSection("validation");
                var validationOptions = configurationSection.Get<ValidationOptions>();

                if (validationOptions is null)
                {
                    throw new InvalidOperationException(Messages.ValidationSectionNotFound);
                }

                if (validationOptions.IsEnabled)
                {
                    var nonDynamicAssemblyList = GetNonDynamicAssemblies();

                    var validatorTypeList = nonDynamicAssemblyList
                        .SelectMany(assembly => assembly.DefinedTypes)
                        .Where(typeInfo
                            => !typeInfo.IsAbstract
                            && typeInfo.BaseType is not null
                            && typeInfo.BaseType.IsGenericType
                            && typeInfo.BaseType.GetGenericTypeDefinition().IsSubclassOf(typeof(AbstractValidator<>)))
                        .ToList();

                    foreach (var validatorTypeItem in validatorTypeList)
                    {
                        if (!validatorTypeItem.Name.EndsWith("Validator"))
                        {
                            throw new InvalidOperationException(string.Format(Messages.ValidatorName, validatorTypeItem.FullName));
                        }
                        else if (!validatorTypeItem.IsSealed)
                        {
                            throw new InvalidOperationException(string.Format(Messages.NotSealedValidator, validatorTypeItem.FullName));
                        }
                    }

                    serviceCollection
                        .AddMvc()
                        .AddFluentValidation(options =>
                        {
                            options.ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
                            options.RegisterValidatorsFromAssemblies(nonDynamicAssemblyList);
                            options.DisableDataAnnotationsValidation = validationOptions.RunDefaultMvcValidationAfterFluentValidationExecutes;
                            options.AutomaticValidationEnabled = validationOptions.AutomaticValidationEnabled;
                        });

                    validatorTypeList
                        .ForEach(typeInfo =>
                        {
                            serviceCollection.AddScoped(typeof(IValidator<>).MakeGenericType(typeInfo), typeInfo);
                        });
                }
            }
            catch
            {
                throw;
            }
        }

        private static List<Assembly> GetNonDynamicAssemblies()
        {
            var currentDomainAssemblyList = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .ToList();

            var fullAssemblyList = new List<Assembly>(currentDomainAssemblyList);

            foreach (var assemblyItem in currentDomainAssemblyList)
            {
                var referencedAssemblyList = assemblyItem
                    .GetReferencedAssemblies()
                    .Select(Assembly.Load)
                    .ToList();

                fullAssemblyList.AddRange(referencedAssemblyList);
            }

            fullAssemblyList = fullAssemblyList
                .Where(assembly => !assembly.IsDynamic)
                .Distinct()
                .ToList();

            return fullAssemblyList;
        }
    }
}