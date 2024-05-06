﻿using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Infra.Repositories.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

namespace Fiap.McTech.Infra.Context
{
	public static class DbConfiguration
	{
		private const string DATABASE_NOT_FOUND_ERROR_MESSAGE = "Database connection not found.";

		public static void ConfigureMySql(this IServiceCollection services, IConfiguration configuration)
		{
			try
			{
				var connectionString = configuration.GetConnectionString("DefaultConnection");

				if (string.IsNullOrWhiteSpace(connectionString))
				{
					Console.WriteLine(DATABASE_NOT_FOUND_ERROR_MESSAGE);
					throw new ArgumentNullException(nameof(connectionString), DATABASE_NOT_FOUND_ERROR_MESSAGE);
				}

				services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Erro durante a configuração do banco de dados.");
				throw;
			}
		}

		public static void RegisterRepositories(this IServiceCollection services)
		{
			try
			{
				//TODO - ADD REPOSITORIES HERE
				services.AddScoped<ICartRepository, CartRepository>();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Erro durante a configuração do banco de dados.");

				throw;
			}
		}
	}
}