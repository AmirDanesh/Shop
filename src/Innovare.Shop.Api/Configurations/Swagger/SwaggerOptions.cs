namespace Innovare.Shop.Api.Configurations.Swagger
{
    public class SwaggerOptions
    {
        public bool IsEnabled { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public string UiPath { get; set; }

        public string Description { get; set; }

        public SecurityOptions Security { get; set; }
    }
}