using AutoMapper;
using Innovare.Shop.Business.Abstractions.Mapper;
using Innovare.Shop.Business.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Innovare.Shop.Business.Configurations.Mapper
{
    public static class ServiceRegistration
    {
        public static void AddMapper<TMapperOptions>(this IServiceCollection serviceCollection) where TMapperOptions : class, IMapperOptions
        {
            using var serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var configurationSection = serviceProvider.GetService<IConfiguration>().GetSection("mapper");
                var mapperOptionsType = typeof(TMapperOptions);
                var mapperOptions = configurationSection.Get<TMapperOptions>();

                if (mapperOptions is null)
                {
                    throw new InvalidOperationException(Messages.MapperSectionNotFound);
                }

                if (!mapperOptionsType.Name.EndsWith("Options"))
                {
                    throw new InvalidOperationException(string.Format(Messages.MapperOptionsName, mapperOptionsType.FullName));
                }

                if (mapperOptions.IsEnabled)
                {
                    serviceCollection.Configure<TMapperOptions>(configurationSection);

                    var profileTypeList = GetDefinedTypes()
                        .Where(typeInfo
                            => typeInfo.IsSubclassOfGeneric(typeof(Mapper<>))
                            && !typeInfo.IsAbstract)
                        .ToList();

                    foreach (var profileTypeItem in profileTypeList)
                    {
                        if (!profileTypeItem.Name.EndsWith("Mapper"))
                        {
                            throw new InvalidOperationException(string.Format(Messages.MapperName, profileTypeItem.FullName));
                        }
                        else if (!profileTypeItem.IsSealed)
                        {
                            throw new InvalidOperationException(string.Format(Messages.NotSealedMapper, profileTypeItem.FullName));
                        }
                    }

                    var mapper = new AutoMapper.MapperConfiguration(mapperConfiguration =>
                    {
                        var profileList = profileTypeList
                            .Select(profileType =>
                            {
                                var profile = Activator.CreateInstance(profileType, serviceProvider.GetService<IOptions<TMapperOptions>>()) as Profile;

                                profile.ShouldMapProperty = prop => prop.GetMethod.IsPublic || prop.GetMethod.IsAssembly;
                                profile.ForAllMaps((typeMap, configuration) => configuration.PreserveReferences());

                                return profile;
                            })
                            .ToList();

                        mapperConfiguration.AddProfiles(profileList);
                    })
                    .CreateMapper();

                    switch (mapperOptions.ServiceLifetime)
                    {
                        case ServiceLifetime.Transient:
                            serviceCollection.AddTransient(serviceProvider => mapper);
                            break;

                        case ServiceLifetime.Scoped:
                            serviceCollection.AddScoped(serviceProvider => mapper);
                            break;

                        case ServiceLifetime.Singleton:
                            serviceCollection.AddSingleton(serviceProvider => mapper);
                            break;

                        default:
                            throw new InvalidOperationException(Messages.MapperServiceLifeTime);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private static List<TypeInfo> GetDefinedTypes()
        {
            var definedTypeList = GetNonDynamicAssemblies()
                    .SelectMany(assembly => assembly.DefinedTypes)
                    .ToList();

            return definedTypeList;
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

        private static bool IsSubclassOfGeneric(this Type type, Type genericType)
        {
            var result = false;

            while (type is not null && type != typeof(object))
            {
                var cur = type.IsGenericType
                    ? type.GetGenericTypeDefinition()
                    : type;

                if (genericType == cur)
                {
                    result = true;

                    break;
                }

                type = type.BaseType;
            }

            return result;
        }
    }
}