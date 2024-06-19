using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Fiap.McTech.Api.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                const string ApiVersion = "v1";
                c.SwaggerDoc(ApiVersion, new OpenApiInfo
                {
                    Title = "McTech API",
                    Version = ApiVersion,
                    Description = "Backend API responsible for the operational control of the McTech snack bar. It manages essential functions such as orders, payments, customers, and other operations to ensure efficiency and improvement of the services offered by the McTech snack bar.\n\n" +
                                  "### Contacts\n" +
                                  "- **Ervin Notari Junior**: ervinnotari@hotmail.com\n" +
                                  "- **Guilherme Novaes da silva**: guilherme.novaes233@gmail.com\n" +
                                  "- **José Maria dos Reis Lisboa**: josemrlisboa@gmail.com\n" +
                                  "- **Vanessa Alves do Nascimento**: vanascimento.dev@gmail.com\n"
                });

                var xmlFiles = new List<string> {
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml",
                    "Fiap.McTech.Application.xml",
                    "Fiap.McTech.Domain.xml"
                };
                foreach (var xmlFile in xmlFiles)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                }
            });
        }

        public static void UseTest(this WebApplication app, ConfigurationManager configuration)
        {
            app.UseSwagger();
            var allowSwaggerUi = configuration.GetValue<bool>("ALLOW_SWAGGER_UI");
            if (app.Environment.IsDevelopment() || allowSwaggerUi)
            {
                app.UseSwaggerUI();
            }
        }
    }
}
