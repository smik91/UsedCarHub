using System.Reflection;
using Microsoft.OpenApi.Models;

namespace UsedCarHub.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "UsedCarHub API",
                    Description = "An ASP.NET Core Web API for managing UsedCarHub",
                    Contact = new OpenApiContact
                    {
                        Name = "Contact GitHub",
                        Url = new Uri("https://github.com/smik91")
                    },
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }
}