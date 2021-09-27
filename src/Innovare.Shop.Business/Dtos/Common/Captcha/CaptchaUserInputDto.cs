using Innovare.Shop.Business.Abstractions.Dto;

namespace Innovare.Shop.Business.Dtos.Common.Captcha
{
    public sealed class CaptchaUserInputDto : DtoBase
    {
        public string Id { get; set; }

        public string Text { get; set; }
    }
}