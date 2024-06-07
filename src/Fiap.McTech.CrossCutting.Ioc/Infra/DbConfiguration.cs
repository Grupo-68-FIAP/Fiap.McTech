using Fiap.McTech.Domain.Exceptions;
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
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Infra.Context
{
    public static class DbConfiguration
    {
        public static void ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                    ?? configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new DatabaseException("Database is not configured. Please inform your connection string.");

                services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error on database condigure.", ex);
            }
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICartClientRepository, CartClientRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        public static void McTechDatabaseInitialize(this IServiceScope scope)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataContext>>();
            logger.LogInformation("Preparing database.");

            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            var pendingMigrations = dbContext.Database.GetPendingMigrations();

            if (!dbContext.Database.CanConnect())
            {
                logger.LogWarning("Database {dbName} not found! Creating the database.", dbContext.Database.GetDbConnection().Database);
                dbContext.Database.Migrate();
            }
            else if (pendingMigrations.Any())
            {
                logger.LogWarning("There are {count} migrations that haven't been run yet. Updating the database.", pendingMigrations.Count());
                dbContext.Database.Migrate();
            }
            logger.LogInformation("Database is prepared.");
        }
    }
}