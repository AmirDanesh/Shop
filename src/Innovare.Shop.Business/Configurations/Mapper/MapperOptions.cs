using Innovare.Shop.Business.Abstractions.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Innovare.Shop.Business.Configurations.Mapper
{
    public sealed class MapperOptions : IMapperOptions
    {
        public bool IsEnabled { get; set; }

        public ServiceLifetime ServiceLifetime { get; set; }

        public string ImageUrlBaseAddress { get; set; }
    }
}