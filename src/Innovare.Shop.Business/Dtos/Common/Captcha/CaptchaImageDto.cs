using Innovare.Shop.Business.Abstractions.Dto;

namespace Innovare.Shop.Business.Dtos.Common.Captcha
{
    public sealed class CaptchaImageDto : DtoBase
    {
        public string Id { get; set; }

        public string Image { get; set; }
    }
}