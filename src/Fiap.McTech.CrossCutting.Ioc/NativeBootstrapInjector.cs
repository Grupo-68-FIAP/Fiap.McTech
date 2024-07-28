using Fiap.McTech.Application.AppServices.Cart;
using Fiap.McTech.Application.AppServices.Clients;
using Fiap.McTech.Application.AppServices.Orders;
using Fiap.McTech.Application.AppServices.Payment;
using Fiap.McTech.Application.AppServices.Products;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Infra.Context;
using Fiap.McTech.Infra.Services.Interfaces;
using Fiap.McTech.Services.Services.MercadoPago;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Fiap.McTech.CrossCutting.Ioc
{
    public static class NativeBootstrapInjector
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Infra 
            services.ConfigureSqlServer(configuration);
            services.RegisterRepositories();

            //SERVICES
            services.AddScoped<IMercadoPagoService, MercadoPagoService>();

            services.AddHttpClient<IMercadoPagoService, MercadoPagoService>((serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(configuration["MercadoPagoConfig:BaseUrl"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["MercadoPagoConfig:AccessToken"]);
                client.DefaultRequestHeaders.Add("X-Idempotency-Key", configuration["MercadoPagoConfig:IdempotencyKey"]);
            });

            //APP Services
            services.AddScoped<IClientAppService, ClientAppService>();
            services.AddScoped<ICartAppService, CartAppService>();
            services.AddScoped<IOrderAppService, OrderAppService>();
            services.AddScoped<IPaymentAppService, PaymentAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
        }
    }
}
