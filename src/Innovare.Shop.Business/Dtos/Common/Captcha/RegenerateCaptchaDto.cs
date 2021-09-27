using Innovare.Shop.Business.Abstractions.Dto;

namespace Innovare.Shop.Business.Dtos.Common.Captcha
{
    public sealed class RegenerateCaptchaDto : DtoBase
    {
        public string OldCaptchaId { get; set; }
    }
}