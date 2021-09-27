using Innovare.Shop.Business.Abstractions.Dto;

namespace Innovare.Shop.Business.Dtos.Common
{
    public sealed class ImageDto : DtoBase
    {
        public string AlternativeText { get; set; }

        public string Url { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string FileFullName { get; set; }
    }
}