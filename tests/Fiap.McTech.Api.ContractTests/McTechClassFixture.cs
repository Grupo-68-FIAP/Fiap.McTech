using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Pipelines;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Fiap.McTech.Api.ContractTests
{
    public class McTechClassFixture : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string SCEHMA = "FakeBearer";

        private static readonly string _key = Guid.NewGuid().ToString();

        protected readonly HttpClient _client;

        public McTechClassFixture(WebApplicationFactory<Program> factory)
        {
            SetUpEnvironmentVariables();

            _client = factory
                .WithWebHostBuilder(builder => builder.ConfigureServices(ConfigureTestServices))
                .CreateClient();
        }

        private static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            var secret = Encoding.UTF8.GetBytes(_key);
            return new SymmetricSecurityKey(secret);
        }

        private static void SetUpEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("OPENID_AUTHORITY", "fake");
            Environment.SetEnvironmentVariable("OPENID_AUDIENCE", "fake");
            Environment.SetEnvironmentVariable("CONNECTION_STRING", "in-memory");
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("ALLOW_SWAGGER_UI", "true");
        }

        private void ConfigureTestServices(IServiceCollection services)
        {
            services.AddAuthentication(SCEHMA)
                .AddJwtBearer(SCEHMA, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = GetSymmetricSecurityKey(),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Headers["Authorization"].ToString().Replace("FakeBearer ", "");
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        protected static string GenerateToken()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Name, "Test User"),
            };

            return GenerateToken(claims);
        }

        protected static string GenerateToken(Claim[] claims)
        {
            var creds = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "fake",
                audience: "fake",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        protected void ClientAuthenticate()
        {
            var token = GenerateToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(SCEHMA, token);
        }

        protected void ClientNoAuthenticate()
        {
            _client.DefaultRequestHeaders.Authorization = null;
        }

    }
}
