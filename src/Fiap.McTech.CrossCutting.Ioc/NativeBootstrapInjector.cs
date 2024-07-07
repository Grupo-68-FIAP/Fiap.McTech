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
using Microsoft.Extensions.Http;

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
            services.Configure<MercadoPagoConfig>(config => { configuration.GetSection(nameof(MercadoPagoConfig)); });
           
            services.AddHttpClient("MercadoPago", client =>
            {
                client.BaseAddress = new Uri(configuration["MercadoPago:BaseUrl"]);
                client.DefaultRequestHeaders.Add("X-Idempotency-Key", configuration["MercadoPago:IdempotencyKey"]);
            });

            services.AddScoped<IMercadoPagoService, MercadoPagoService>();

            //APP Services
            services.AddScoped<IClientAppService, ClientAppService>();
            services.AddScoped<ICartAppService, CartAppService>();
            services.AddScoped<IOrderAppService, OrderAppService>();
            services.AddScoped<IPaymentAppService, PaymentAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
        }
    }
}
