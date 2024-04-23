using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.McTech.CrossCutting.Ioc
{
	public static class NativeBootstrapInjector
	{
		public static void RegisterServices(this IServiceCollection services, IConfiguration config)
		{
			// Infra 

			//APP Services
			services.AddScoped<IUserAppService, UserAppService>();
		}
	}
}