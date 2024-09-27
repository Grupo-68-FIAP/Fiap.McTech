
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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
        public static void AddOpenId(this IServiceCollection services, ConfigurationManager configuration)
        {
            var openIdAuthority = configuration.GetValue<string>("OPENID_AUTHORITY");
            var clientId = configuration.GetValue<string>("OPENID_CLIENT_ID");
            var clientSecret = configuration.GetValue<string>("OPENID_CLIENT_SECRET");

            UseOpenIdConnect = !string.IsNullOrEmpty(openIdAuthority) && !string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            if (!UseOpenIdConnect)
            {
                return;
            }

            var cli = new HttpClient();
            cli.


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.SameSite = SameSiteMode.Strict;
            })
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Authority = openIdAuthority;
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.ResponseMode = OpenIdConnectResponseMode.Query;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;

                string scopeString = "openid profile email";
                scopeString.Split(" ", StringSplitOptions.TrimEntries).ToList().ForEach(scope => options.Scope.Add(scope));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Authority,
                    ValidAudience = options.ClientId
                };

                options.SaveTokens = true;
            });
        }

        /// <summary>
        /// Add OpenIdConnect configuration to the application.
        /// </summary>
        /// <param name="app">WebApplication instance.</param>
        public static void UseOpenId(this IApplicationBuilder app)
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
