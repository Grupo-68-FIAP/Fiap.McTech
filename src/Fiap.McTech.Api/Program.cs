using Fiap.McTech.Api.Configurations;
using Fiap.McTech.Api.Handlers;
using Fiap.McTech.CrossCutting.Ioc;
using Fiap.McTech.CrossCutting.Ioc.Mappers;
using Fiap.McTech.Infra.Context;
using Fiap.McTech.Services.Services.MercadoPago;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddCommandLine(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwtBearerAuthentication(builder.Configuration);
builder.Services.AddSwagger();

builder.Services.Configure<MercadoPagoConfig>(builder.Configuration.GetSection("MercadoPagoConfig"));

// AutoMapper configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.RegisterMappings();
builder.Services.RegisterServices(builder.Configuration);

// Cors configuration
builder.Services.AddCors(options =>
{
    var allowOrigins = builder.Configuration.GetValue<string>("ALLOW_ORIGINS") ?? "*";
    options.AddPolicy("CorsConfig", builder => builder.WithOrigins(allowOrigins.Split(';')).AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
scope.McTechDatabaseInitialize();

app.UseSwagger(builder.Configuration);

app.UseCors("CorsConfig");

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuth();

app.MapControllers();

await app.RunAsync();
