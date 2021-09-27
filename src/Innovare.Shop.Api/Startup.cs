using Innovare.Shop.Api.Configurations.Swagger;
using Innovare.Shop.Business.Configurations.Data;
using Innovare.Shop.Business.Configurations.Identitiy;
using Innovare.Shop.Business.Configurations.Mapper;
using Innovare.Shop.Business.Configurations.Service;
using Innovare.Shop.Business.Configurations.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Innovare.Shop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataConfigurations();
            services.AddMappers();
            services.AddServices();
            //serviceCollection.AddBackgroundServices();
            services.AddValidation();
            //serviceCollection.AddModelViewController();
            services.AddControllers();
            //serviceCollection.AddGlobalization();
            //serviceCollection.AddVersioning();
            services.AddSwagger();
            //services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Innovare.Shop.Api", Version = "v1" }));
            //serviceCollection.AddRouteConstraints();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDatabases();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
            }
            //app.UseGlobalization();
            //app.UseBackgroundServices();
            //app.UseCors("AllowAll");
            app.UseIdentity();
            //app.UseModelViewController();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}