using Innovare.Shop.Business.Abstractions.Dto;

namespace Innovare.Shop.Business.Dtos.V00_00.Identity
{
    public sealed class CurrentUserDto : DtoBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public string NationalCode { get; set; }
    }
}