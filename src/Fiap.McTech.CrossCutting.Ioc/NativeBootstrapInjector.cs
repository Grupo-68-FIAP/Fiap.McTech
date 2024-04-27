using Fiap.McTech.Application.Services;
using Fiap.McTech.Domain.Interfaces.AppServices;
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