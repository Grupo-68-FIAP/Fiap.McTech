using Fiap.McTech.Api.Handlers;
using Fiap.McTech.CrossCutting.Ioc;
using Fiap.McTech.CrossCutting.Ioc.Mappers;
using Fiap.McTech.Infra.Context;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddCommandLine(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
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
                      "- **Vanessa Alves do Nascimento**: vanascimento.dev@gmail.com\n",
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
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

// Configurando o AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.RegisterMappings();

builder.Services.RegisterServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
scope.McTechDatabaseInitialize();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevelopmentPolicy");
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
