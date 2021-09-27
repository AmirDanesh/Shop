using AutoMapper;
using Microsoft.Extensions.Options;
using System;

namespace Innovare.Shop.Business.Abstractions.Mapper
{
    public abstract class Mapper<TMapperOptions> : Profile where TMapperOptions : class, IMapperOptions
    {
        public Mapper(IOptions<TMapperOptions> mapperOptions)
        {
            MapperOptions = mapperOptions?.Value ?? throw new InvalidOperationException(nameof(MapperOptions));
        }

        protected TMapperOptions MapperOptions { get; }
    }
}