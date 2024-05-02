using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Infra.Repositories.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.McTech.Infra.Context
{
    public static class DbConfiguration
    {
        private const string DATABASE_NOT_FOUND_ERROR_MESSAGE = "Database connection not found.";


        public static void MySqlConfigure(this IServiceCollection services)
        {
            var connectionString = GetConnectionString();

            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<ICartRepository, CartRepository>();
        }

        internal static string GetConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
            // TODO: Adicionar suporte ao arquivo de configurações caso a variavel de ambiente não estiver populada
            if (connectionString != null) return connectionString;
            else throw new Exception(DATABASE_NOT_FOUND_ERROR_MESSAGE);
        }
    }
}
