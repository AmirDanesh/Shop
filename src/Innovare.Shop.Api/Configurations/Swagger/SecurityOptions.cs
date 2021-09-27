using NSwag;

namespace Innovare.Shop.Api.Configurations.Swagger
{
    public class SecurityOptions
    {
        public bool IsEnabled { get; set; }

        public OpenApiSecuritySchemeType Type { get; set; }

        public string Scheme { get; set; }

        public string Name { get; set; }

        public OpenApiSecurityApiKeyLocation In { get; set; }

        public string Description { get; set; }
    }
}