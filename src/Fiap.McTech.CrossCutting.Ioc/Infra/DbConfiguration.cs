using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;
using Fiap.McTech.Infra.Repositories.Cart;
using Fiap.McTech.Infra.Repositories.Clients;
using Fiap.McTech.Infra.Repositories.Orders;
using Fiap.McTech.Infra.Repositories.Payments;
using Fiap.McTech.Infra.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.McTech.Infra.Context
{
    public static class DbConfiguration
    {
        private const string DATABASE_NOT_FOUND_ERROR_MESSAGE = "Database connection not found.";

        public static void ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration)
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
                services.AddScoped<ICartClientRepository, CartClientRepository>();
                services.AddScoped<ICartItemRepository, CartItemRepository>();
                services.AddScoped<IProductRepository, ProductRepository>();
                services.AddScoped<IClientRepository, ClientRepository>();
                services.AddScoped<IPaymentRepository, PaymentRepository>();
                services.AddScoped<IOrderRepository, OrderRepository>();
            }
            catch (Exception)
            {
                Console.WriteLine("Erro durante a configuração do banco de dados.");
                throw;
            }
        }

        public static void DbStart(this IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            var pendingMigrations = dbContext.Database.GetPendingMigrations();

            // Verifica se o banco de dados existe e aplica migrações
            if (!dbContext.Database.CanConnect())
            {
                Console.WriteLine("Database {0} not found! Waiting while creating...", dbContext.Database.GetDbConnection().Database);
                dbContext.Database.Migrate();
            }
            else if (pendingMigrations.Any())
            {
                Console.WriteLine("There are {0} migrations that haven't been run yet. Waiting while your database is updated.", pendingMigrations.Count());
                dbContext.Database.Migrate();
            }
        }
    }
}