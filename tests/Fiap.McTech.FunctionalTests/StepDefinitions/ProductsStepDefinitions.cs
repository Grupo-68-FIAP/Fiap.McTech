using Fiap.McTech.Api.Controllers.Product;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Products.Add;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Fiap.McTech.FunctionalTests.StepDefinitions
{
    [Binding]
    public class ProductsStepDefinitions
    {
        private string _name = string.Empty;
        private decimal _value;
        private string _description = string.Empty;
        private string _image = string.Empty;
        private ProductCategory _category;
        private Product? _product;
        private Exception? _exception;
        private readonly ProductController _controller;
        private IActionResult? _response;
        private Guid _productId;

        public ProductsStepDefinitions(IProductAppService productAppService)
        {
            _controller = new ProductController(productAppService);
        }

        [Given(@"que eu inseri ""(.*)"" como o nome do produto")]
        public void DadoQueEuInseriComoONomeDoProduto(string name)
        {
            _name = name;
        }

        [Given(@"que eu inseri (.*) como o valor do produto")]
        public void DadoQueEuInseriComoOValorDoProduto(decimal value)
        {
            _value = value;
        }

        [Given(@"que eu inseri ""(.*)"" como a descrição do produto")]
        public void DadoQueEuInseriComoADescricaoDoProduto(string description)
        {
            _description = description;
        }

        [Given(@"que eu inseri ""(.*)"" como a imagem do produto")]
        public void DadoQueEuInseriComoAImagemDoProduto(string image)
        {
            _image = image;
        }

        [Given(@"que eu selecionei ""(.*)"" como a categoria do produto")]
        public void DadoQueEuSelecioneiComoACategoriaDoProduto(string category)
        {
            _category = Enum.Parse<ProductCategory>(category);
        }

        [When(@"eu crio o produto")]
        public async Task QuandoEuCrioOProduto()
        {
            try
            {
                var productDto = new CreateProductInputDto(_name, _value, _description, _category);
                _response = await _controller.CreateProduct(productDto);
                var result = _response as CreatedAtActionResult;
                _productId = (Guid) (result?.Value ?? Guid.Empty);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"o produto deve ser criado com sucesso")]
        public void EntaoOProdutoDeveSerCriadoComSucesso()
        {
            Assert.NotNull(_response);
            var result = _response as CreatedAtActionResult;
            Assert.NotNull(result);
            Assert.Equal(201, result?.StatusCode);
        }

        [Then(@"a criação do produto deve falhar com ""(.*)""")]
        public void EntaoACriacaoDoProdutoDeveFalharCom(string errorMessage)
        {
            Assert.NotNull(_exception);
            Assert.Contains(errorMessage, _exception?.Message);
        }

        [Given("que um produto existe no sistema")]
        public async Task GivenQueUmProdutoExisteNoSistema()
        {
            var productDto = new CreateProductInputDto("Produto Exemplo", 10, "Descrição do Produto Exemplo", ProductCategory.Snack);
            var result = await _controller.CreateProduct(productDto) as CreatedAtActionResult;
            _productId = (Guid) (result?.Value ?? Guid.Empty);
        }

        [When("eu removo o produto")]
        public async Task WhenEuRemovoOProduto()
        {
            _response = await _controller.DeleteProduct(_productId);
        }

        [Then("o produto não deve mais existir no sistema")]
        public async Task ThenOProdutoNaoDeveMaisExistirNoSistema()
        {
            var result = await _controller.GetProduct(_productId) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Then("o status da resposta deve ser 204 No Content")]
        public void ThenOStatusDaRespostaDeveSerNoContent()
        {
            var noContentResult = _response as NoContentResult;
            Assert.Equal(204, noContentResult?.StatusCode);
        }

        [When("eu atualizo o produto com novos dados")]
        public async Task WhenEuAtualizoOProdutoComNovosDados()
        {
            var updateProductDto = new UpdateProductInputDto(
                _productId,
                "Produto Atualizado",
                20.0M,
                "Descrição Atualizada",
                "imagem_atualizada.jpg",
                ProductCategory.Snack
            );
            _response = await _controller.UpdateProduct(_productId, updateProductDto);
        }

        [Then("o produto deve ser atualizado com sucesso")]
        public async Task ThenOProdutoDeveSerAtualizadoComSucesso()
        {
            var result = await _controller.GetProduct(_productId) as OkObjectResult;
            var updatedProduct = result?.Value as ProductOutputDto;
            Assert.NotNull(updatedProduct);
            Assert.Equal("Produto Atualizado", updatedProduct?.Name);
            Assert.Equal(20.0M, updatedProduct?.Value);
            Assert.Equal("Descrição Atualizada", updatedProduct?.Description);
            Assert.Equal("imagem_atualizada.jpg", updatedProduct?.Image);
            Assert.Equal(ProductCategory.Snack, updatedProduct?.Category);
        }

        [Then("o status da resposta deve ser 200 OK")]
        public void ThenOStatusDaRespostaDeveSer200OK()
        {
            var okResult = _response as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
        }
    }
}
