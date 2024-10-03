using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace Fiap.McTech.Api.ContractTests
{
    public class ClientContractTests : McTechClassFixture
    {
        public ClientContractTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task Get_AllClients_Returns_201NoContent()
        {
            ClientAuthenticate();

            var response = await _client.GetAsync("/api/client");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Get_AllClients_Returns_401Unauthorized()
        {
            ClientNoAuthenticate();

            var response = await _client.GetAsync("/api/client");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Post_Client_Returns_404BadRequest_Validations()
        {
            ClientAuthenticate();

            var body = new { Name = "test name", Cpf = "invalid", Email = "invalid" };
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/client", content);

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("The field Cpf must be a string or array type with a minimum length of '11'.", jsonObject.SelectToken("$.errors.Cpf[0]")?.ToString());
            Assert.Equal("Invalid Cpf.", jsonObject.SelectToken("$.errors.Cpf[1]")?.ToString());
            Assert.Equal("Invalid Email.", jsonObject.SelectToken("$.errors.Email[0]")?.ToString());
        }
    }
}
