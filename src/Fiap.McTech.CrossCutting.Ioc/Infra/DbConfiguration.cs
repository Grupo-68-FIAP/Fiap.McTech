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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data.Common;

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
            const int maxRetryAttempts = 3;
            var tryCount = 0;
            var dbConnection = false;

            do
            {
                try
                {
                    dbContext.Database.ExecuteSqlRaw("SELECT 1;");
                    dbConnection = true;
                }
                catch (SqlException ex)
                {
                    if(ex.ClientConnectionId == Guid.Empty)
                    {
                        tryCount++;
                        logger.LogWarning(ex, "Attempt {TryCount} of {MaxRetryAttempts}: Database connection failed. Retrying in 10 second...", tryCount, maxRetryAttempts);
                        Task.Delay(10000).Wait();
                    }
                    else
                    {
                        dbConnection = true;
                    }
                }
            } while (!dbConnection && tryCount < maxRetryAttempts);
            if (!dbConnection)
            {
                logger.LogError("Database connection failed after {MaxRetryAttempts} attempts. Exiting application.", maxRetryAttempts);
                throw new InvalidOperationException("Database connection failed.");
            }

            if (!dbContext.Database.CanConnect())
            {
                logger.LogWarning("Database {DbName} not found! Creating the database.", dbContext.Database.GetDbConnection().Database);
                dbContext.Database.Migrate();
            }

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                logger.LogWarning("There are {Count} migrations that haven't been run yet. Updating the database.", pendingMigrations.Count());
                dbContext.Database.Migrate();
            }

            logger.LogInformation("Database is prepared.");
        }
    }
}
