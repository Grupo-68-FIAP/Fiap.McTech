using Fiap.McTech.Application.AppServices.Cart;
using Fiap.McTech.Application.AppServices.Catalog;
using Fiap.McTech.Application.AppServices.Clients;
using Fiap.McTech.Application.AppServices.Orders;
using Fiap.McTech.Application.AppServices.Payment;
using Fiap.McTech.Application.Interfaces.AppServices; 
using Fiap.McTech.Infra.Context; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.CrossCutting.Ioc
{
    public static class NativeBootstrapInjector
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
			// Infra 
			services.ConfigureMySql(configuration);
            services.RegisterRepositories();

			//APP Services
			services.AddScoped<IClientAppService, ClientAppService>();
            services.AddScoped<ICartAppService, CartAppService>();
            services.AddScoped<ICatalogAppService, CatalogAppService>();
            services.AddScoped<IOrderAppService, OrderAppService>();
            services.AddScoped<IPaymentAppService, PaymentAppService>();        
        }
    }
}