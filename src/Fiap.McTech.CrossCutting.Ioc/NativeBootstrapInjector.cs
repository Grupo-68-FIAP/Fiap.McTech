using Fiap.McTech.Application.AppServices.Cart;
using Fiap.McTech.Application.AppServices.Clients;
using Fiap.McTech.Application.AppServices.Orders;
using Fiap.McTech.Application.AppServices.Payment;
using Fiap.McTech.Application.AppServices.Product;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Infra.Context;
using Fiap.McTech.Infra.Services;
using Fiap.McTech.Infra.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IPayPalPaymentService, PayPalPaymentService>();

            //APP Services
            services.AddScoped<IClientAppService, ClientAppService>();
            services.AddScoped<ICartAppService, CartAppService>();
            services.AddScoped<IOrderAppService, OrderAppService>();
            services.AddScoped<IPaymentAppService, PaymentAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
        }
    }
}