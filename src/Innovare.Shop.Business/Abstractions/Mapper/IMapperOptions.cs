using Microsoft.Extensions.DependencyInjection;

namespace Innovare.Shop.Business.Abstractions.Mapper
{
    public interface IMapperOptions
    {
        bool IsEnabled { get; set; }

        ServiceLifetime ServiceLifetime { get; set; }
    }
}