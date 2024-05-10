using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;
using Fiap.McTech.Infra.Repositories.Cart;
using Fiap.McTech.Infra.Repositories.Clients;
using Fiap.McTech.Infra.Repositories.Products;
using Microsoft.Data.SqlClient;
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
				var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
					?? configuration.GetConnectionString("DefaultConnection");

				if (string.IsNullOrWhiteSpace(connectionString))
				{
					Console.WriteLine(DATABASE_NOT_FOUND_ERROR_MESSAGE);
					throw new ArgumentNullException(nameof(connectionString), DATABASE_NOT_FOUND_ERROR_MESSAGE);
				}

				services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

				_ = TestConnection(connectionString);
            }
			catch (Exception)
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
				services.AddScoped<ICartClientRepository, CartClientRepository>();
				services.AddScoped<ICartItemRepository, CartItemRepository>();
				services.AddScoped<IProductRepository, ProductRepository>();
				services.AddScoped<IClientRepository, ClientRepository>();
			}
			catch (Exception)
			{
				Console.WriteLine("Erro durante a configuração do banco de dados.");
				throw;
			}
		}

        public static async Task TestConnection(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                Console.WriteLine("Conexão com o banco de dados bem-sucedida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao testar a conexão: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}