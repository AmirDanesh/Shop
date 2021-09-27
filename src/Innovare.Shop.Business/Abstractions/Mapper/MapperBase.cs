using Innovare.Shop.Business.Configurations.Mapper;
using Microsoft.Extensions.Options;
using System;

namespace Innovare.Shop.Business.Abstractions.Mapper
{
    internal abstract class MapperBase : Mapper<MapperOptions>
    {
        public MapperBase(IOptions<MapperOptions> mapperOptions) : base(mapperOptions)
        {
            ObjectName = GetType().Name.Replace("Mapper", string.Empty);
        }

        protected string ObjectName { get; private set; }

        //protected DateTimeOffset CurrentUtcDateTimeOffset => DateTime.Now.UtcDateTimeOffset();

        //protected DateTimeOffset CurrentLocalDateTimeOffset => DateTime.Now.LocalDateTimeOffset();

        //protected DateTime CurrentUtcDate => DateTime.Now.UtcDateTimeOffset().Date;

        //protected DateTime CurrentLocalDate => DateTime.Now.LocalDateTimeOffset().Date;

        //protected TimeSpan CurrentUtcTime => DateTime.Now.UtcDateTimeOffset().TimeOfDay;

        //protected TimeSpan CurrentLocalTime => DateTime.Now.LocalDateTimeOffset().TimeOfDay;

        protected DateTimeOffset? NullDateTimeOffset => null as DateTimeOffset?;

        protected DateTime? NullDateTime => null as DateTime?;

        //protected ImageDto GenerateImageMetadata(string filename, string alternativeText)
        //{
        //    filename = filename.ToLower();

        //    var imageDto = new ImageDto()
        //    {
        //        AlternativeText = alternativeText,
        //        Url = GenerateFileUrl(filename),
        //        FileName = Path.GetFileNameWithoutExtension(filename),
        //        FileExtension = Path.GetExtension(filename).Replace(".", string.Empty),
        //        FileFullName = filename
        //    };

        //    return imageDto;
        //}

        //protected ImageDto GenerateImageThumbnailMetadata(string filename, string alternativeText)
        //{
        //    filename = $"thumb_{filename}".ToLower();

        //    var imageDto = new ImageDto()
        //    {
        //        AlternativeText = alternativeText,
        //        Url = GenerateFileUrl(filename),
        //        FileName = Path.GetFileNameWithoutExtension(filename),
        //        FileExtension = Path.GetExtension(filename).Replace(".", string.Empty),
        //        FileFullName = filename
        //    };

        //    return imageDto;
        //}        //protected ImageDto GenerateImageMetadata(string filename, string alternativeText)
        //{
        //    filename = filename.ToLower();

        //    var imageDto = new ImageDto()
        //    {
        //        AlternativeText = alternativeText,
        //        Url = GenerateFileUrl(filename),
        //        FileName = Path.GetFileNameWithoutExtension(filename),
        //        FileExtension = Path.GetExtension(filename).Replace(".", string.Empty),
        //        FileFullName = filename
        //    };

        //    return imageDto;
        //}

        //protected ImageDto GenerateImageThumbnailMetadata(string filename, string alternativeText)
        //{
        //    filename = $"thumb_{filename}".ToLower();

        //    var imageDto = new ImageDto()
        //    {
        //        AlternativeText = alternativeText,
        //        Url = GenerateFileUrl(filename),
        //        FileName = Path.GetFileNameWithoutExtension(filename),
        //        FileExtension = Path.GetExtension(filename).Replace(".", string.Empty),
        //        FileFullName = filename
        //    };

        //    return imageDto;
        //}

        protected string GenerateFileUrl(string filename)
        {
            var url = $"{MapperOptions.ImageUrlBaseAddress}/{Guid.Empty}/{ObjectName}/{filename}".ToLower();

            return url;
        }
    }
}