using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Fiap.McTech.Api.Configurations
{
    /// <summary>
    /// OpenId configuration class.
    /// </summary>
    public static class OpenIdConfig
    {
        private static bool UseOpenIdConnect = false;

        /// <summary>
        /// Add OpenIdConnect configuration to the application.
        /// </summary>
        /// <param name="services">ServiceCollection instance.</param>
        /// <param name="configuration">ConfigurationManager instance.</param>
        public static void AddJwtBearerAuthentication(this IServiceCollection services, ConfigurationManager configuration)
        {
            var openIdAuthority = configuration.GetValue<string>("OPENID_AUTHORITY");
            var clientId = configuration.GetValue<string>("OPENID_CLIENT_ID");
            var clientSecret = configuration.GetValue<string>("OPENID_CLIENT_SECRET");

            UseOpenIdConnect = !string.IsNullOrEmpty(openIdAuthority) && !string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret);

            if (!UseOpenIdConnect)
            {
                return;
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = openIdAuthority;
                    options.Audience = clientId;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = openIdAuthority,
                        ValidAudience = clientId,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                    };
                });
        }

        /// <summary>
        /// Add OpenIdConnect configuration to the application.
        /// </summary>
        /// <param name="app">WebApplication instance.</param>
        public static void UseAuth(this IApplicationBuilder app)
        {
            if (!UseOpenIdConnect)
            {
                return;
            }

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
