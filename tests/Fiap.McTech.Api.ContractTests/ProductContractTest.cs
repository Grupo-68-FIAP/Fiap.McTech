using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Fiap.McTech.Api.ContractTests
{
    public class ProductContractTests : McTechClassFixture
    {
        public ProductContractTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task Get_Produt_Returns_201NoContent()
        {
            ClientNoAuthenticate();

            var response = await _client.GetAsync("/api/product");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

    }
}
