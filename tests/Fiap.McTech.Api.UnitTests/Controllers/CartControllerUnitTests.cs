﻿using AutoMapper;
using Fiap.McTech.Api.Controllers.Cart;
using Fiap.McTech.Application.AppServices.Cart;
using Fiap.McTech.Application.AppServices.Clients;
using Fiap.McTech.Application.AppServices.Products;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;
using Fiap.McTech.Domain.ValuesObjects;
using Fiap.McTech.Infra.Repositories.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace Fiap.McTech.Api.UnitTests.Controllers
{
    public class CartControllerUnitTests
    {
        const string GUID_1 = "a07a97eb-8ead-467c-b3db-35e7d75975c7";
        const string GUID_2 = "1d65c6c3-936a-48d1-8901-d9a09c93bc8b";

        readonly Mock<ICartClientRepository> _mockedCartClientRepository;
        readonly Mock<IClientRepository> _mockedClientRepository;
        readonly Mock<IProductRepository> _mockedProductRepository;

        readonly Mock<ILogger<CartAppService>> _mockedLogger;
        readonly IMapper _mapper;

        public CartControllerUnitTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CartClientProfile>();
                cfg.AddProfile<ClientProfile>();
                cfg.AddProfile<ProductProfile>();
            });
            _mapper = new Mapper(configuration);
            _mockedLogger = new Mock<ILogger<CartAppService>>();

            _mockedCartClientRepository = new Mock<ICartClientRepository>();
            _mockedClientRepository = new Mock<IClientRepository>();
            _mockedProductRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GetCart_Returns_200OK()
        {
            // Arrange
            var c = new CartClient(null, 0);
            _mockedCartClientRepository
                .Setup(repository => repository.GetByCartIdAsync(c.Id))
                .ReturnsAsync(() => c);
            var controller = CreateCartController();

            // Act
            var task = await controller.GetCart(c.Id);

            // Assert
            Assert.IsType<OkObjectResult>(task);
            var taskResult = task as OkObjectResult;
            Assert.IsType<CartClientOutputDto>(taskResult?.Value);
            var objectResult = taskResult.Value as dynamic;
            Assert.Equal(c.Id, objectResult?.Id);
        }

        [Fact]
        public async Task GetCart_Throws_EntityNotFoundException()
        {
            // Arrange
            _mockedCartClientRepository
                .Setup(repository => repository.GetByCartIdAsync(Guid.NewGuid()))
                .ReturnsAsync(() => null);
            var controller = CreateCartController();
            var guidSearch = Guid.NewGuid();

            // Act & Assert
            var task = await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.GetCart(guidSearch));
            Assert.Contains(guidSearch.ToString(), task.Message);
        }

        [Fact]
        public async Task GetCartByClientId_Returns_200OK()
        {
            // Arrange
            var client = new Client("", new Cpf(""), new Email(""));
            var cart = new CartClient(null, 0);
            _mockedCartClientRepository
                .Setup(repository => repository.GetByClientIdAsync(client.Id))
                .ReturnsAsync(() => cart);
            var controller = CreateCartController();

            // Act
            var task = await controller.GetCartByClientId(client.Id);

            // Assert
            Assert.IsType<OkObjectResult>(task);
            var taskResult = task as OkObjectResult;
            Assert.IsType<CartClientOutputDto>(taskResult?.Value);
            var objectResult = taskResult.Value as dynamic;
            Assert.Equal(cart.Id, objectResult?.Id);
        }

        [Fact]
        public async Task GetByCartIdAsync_Throws_EntityNotFoundException()
        {
            // Arrange
            var guidSearch = Guid.NewGuid();
            _mockedCartClientRepository
                .Setup(repository => repository.GetByClientIdAsync(Guid.NewGuid()))
                .ReturnsAsync(() => null);
            var controller = CreateCartController();

            // Act & Assert
            var task = await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.GetCartByClientId(guidSearch));
            Assert.Contains(guidSearch.ToString(), task.Message);
        }

        [Theory]
        [InlineData(true)]
        //[InlineData(false)]
        public async Task CreateCart_Returns_201Created(bool includeClient)
        {
            // Arrange
            var product = new Product("Prod 1", 10, "Prod desc 1", "img", ProductCategory.Snack);
            var client = new Client("Test", new(""), new(""));
            var cartClientInputDto = new CartClientInputDto()
            {
                // need acept card with no client
                ClientId = includeClient ? client.Id : null,
                Items = new() {
                    { new (){ ProductId = product.Id, Quantity = 5 } }
                }
            };
            _mockedCartClientRepository
                .Setup(repository => repository.AddAsync(It.IsAny<CartClient>()))
                .ReturnsAsync<CartClient, ICartClientRepository, CartClient>(x => x);
            _mockedClientRepository
                .Setup(repository => repository.GetByIdAsync(client.Id))
                .ReturnsAsync(() => client);
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(product.Id))
                .ReturnsAsync(() => product);
            var controller = CreateCartController();

            // Act
            var task = await controller.CreateCart(cartClientInputDto, null);

            // Assert
            Assert.IsType<CreatedAtActionResult>(task.Result);
            var taskResult = task.Result as CreatedAtActionResult;
            Assert.IsType<CartClientOutputDto>(taskResult?.Value);
            var objectResult = taskResult.Value as dynamic;
            Assert.NotNull(objectResult?.Id);
            Assert.Equal(50, objectResult?.AllValue);
            Assert.Equal(product.Name, objectResult?.Items[0].Name);
            Assert.Equal(product.Value, objectResult?.Items[0].Value);
            _mockedCartClientRepository.Verify(repository => repository.AddAsync(It.IsAny<CartClient>()), Times.Exactly(1));
        }

        [Fact]
        public async Task CreateCart_Returns_401InvalidToken()
        {
            // Arrange
            var product = new Product("Prod 1", 10, "Prod desc 1", "img", ProductCategory.Snack);
            var cartClientInputDto = new CartClientInputDto()
            {
                Items = new() {
                    { new (){ ProductId = product.Id, Quantity = 5 } }
                }
            };
            var controller = CreateCartController();
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var task = await controller.CreateCart(cartClientInputDto, "<invalid token>");

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(task.Result);
            var taskResult = task.Result as UnauthorizedObjectResult;
            Assert.Equal(401, taskResult?.StatusCode);
            _mockedCartClientRepository.Verify(repository => repository.AddAsync(It.IsAny<CartClient>()), Times.Never);
        }


        [Fact]
        public async Task CreateCart_Returns_201_WithClientIsAuthenticated()
        {
            // Arrange
            var product = new Product("Prod 1", 10, "Prod desc 1", "img", ProductCategory.Snack);
            var cartClientInputDto = new CartClientInputDto()
            {
                Items = new() {
                    { new (){ ProductId = product.Id, Quantity = 5 } }
                }
            };
            var cpf = "49380966091";
            var c = new Client("Test 2", new(cpf), new("test2@test.com"));
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetClientAsync(c.Cpf))
                .ReturnsAsync(() => c);
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetByIdAsync(c.Id))
                .ReturnsAsync(() => c);
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(product.Id))
                .ReturnsAsync(() => product);
            _mockedCartClientRepository
                .Setup(repository => repository.AddAsync(It.IsAny<CartClient>()))
                .ReturnsAsync<CartClient, ICartClientRepository, CartClient>(x => x);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim("preferred_username", cpf)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext { User = principal };

            var controller = CreateCartController();
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var task = await controller.CreateCart(cartClientInputDto, "<fake valid token>");

            // Assert
            Assert.IsType<CreatedAtActionResult>(task.Result);
            var taskResult = task.Result as CreatedAtActionResult;
            Assert.IsType<CartClientOutputDto>(taskResult?.Value);
            var objectResult = taskResult.Value as dynamic;
            Assert.Equal(c.Id, objectResult?.ClientId);
            _mockedCartClientRepository.Verify(repository => repository.AddAsync(It.IsAny<CartClient>()), Times.Exactly(1));
        }

        [Theory]
        // Test exception when client not found
        [InlineData(GUID_2, GUID_2, typeof(EntityNotFoundException))]
        // Test exception when product not found
        [InlineData(GUID_1, GUID_2, typeof(EntityNotFoundException))]
        // Test exception when invalid cart data
        [InlineData(GUID_1, GUID_1, typeof(EntityValidationException))]
        public async Task CreateCart_Throws_CorrectException(string clientGuid, string productGuid, Type expectedExceptionType)
        {
            // Arrange
            var product = new Product("", 10, "", "", ProductCategory.Snack);
            var client = new Client("Test", new(""), new(""));
            var cartClientInputDto = new CartClientInputDto()
            {
                // need acept card with no client
                ClientId = new Guid(clientGuid),
                Items = new() {
                    { new (){ ProductId = new Guid(productGuid), Quantity = 5 } }
                }
            };
            _mockedClientRepository
                .Setup(repository => repository.GetByIdAsync(new Guid(GUID_1)))
                .ReturnsAsync(() => client);
            _mockedProductRepository
                .Setup(service => service.GetByIdAsync(new Guid(GUID_1)))
                .ReturnsAsync(() => product);
            var controller = CreateCartController();

            // Act & Assert
            await Assert.ThrowsAsync(expectedExceptionType, () => controller.CreateCart(cartClientInputDto, null));
            _mockedCartClientRepository.Verify(repository => repository.AddAsync(It.IsAny<CartClient>()), Times.Never);
        }

        [Fact]
        public async Task AddCartItemToCartClientAsync_Returns_200OK()
        {
            // Arrange
            var prodId = Guid.NewGuid();
            var cart = new CartClient(null, 5);
            cart.Items.Add(new CartClient.Item("Existent item", 1, 5, prodId, Guid.Empty));
            var product = new Product("New product 1", 10, "desc", "img", ProductCategory.Snack);
            _mockedCartClientRepository
                .Setup(repository => repository.GetByCartIdAsync(cart.Id))
                .ReturnsAsync(() => cart);
            _mockedProductRepository
                .Setup(service => service.GetByIdAsync(product.Id))
                .ReturnsAsync(() => product);
            var controller = CreateCartController();

            // Act
            var task = await controller.AddCartItemToCartClientAsync(cart.Id, product.Id);

            // Assert
            Assert.IsType<OkObjectResult>(task);
            var taskResult = task as OkObjectResult;
            Assert.IsType<CartClientOutputDto>(taskResult?.Value);
            var objectResult = taskResult.Value as dynamic;
            Assert.NotNull(objectResult?.Id);
            Assert.Equal(15, objectResult?.AllValue);
            _mockedCartClientRepository.Verify(repository => repository.UpdateAsync(It.IsAny<CartClient>()), Times.Exactly(1));
        }

        [Theory]
        // Test exception when cart not found
        [InlineData(GUID_2, GUID_2)]
        // Test exception when product not found
        [InlineData(GUID_1, GUID_2)]
        public async Task AddCartItemToCartClientAsync_Throws_CorrectException(string cartGuid, string productGuid)
        {
            // Arrange
            var cart = new CartClient(null, 0);
            cart.Items.Add(new CartClient.Item("Prod 1", 1, 10, new Guid(productGuid), Guid.Empty));
            var product = new Product("Product 1", 10, "desc", "ing", ProductCategory.Snack);
            _mockedCartClientRepository
                .Setup(repository => repository.GetByCartIdAsync(new Guid(GUID_1)))
                .ReturnsAsync(() => cart);
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(new Guid(GUID_1)))
                .ReturnsAsync(() => product);
            var controller = CreateCartController();

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.AddCartItemToCartClientAsync(new Guid(cartGuid), new Guid(productGuid)));
            _mockedCartClientRepository.Verify(repository => repository.AddAsync(It.IsAny<CartClient>()), Times.Never);
        }

        [Fact]
        public async Task RemoveCartItemFromCartClientAsync_Returns_200OK()
        {
            // Arrange
            var cart = new CartClient(null, 0);
            var cartItem = new CartClient.Item("Prod 1", 1, 10, Guid.NewGuid(), cart.Id);
            cart.Items.Add(cartItem);
            _mockedCartClientRepository
                .Setup(repository => repository.GetByCartIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cart);
            var controller = CreateCartController();

            // Act
            var task = await controller.RemoveCartItemFromCartClientAsync(cart.Id, cartItem.ProductId);

            // Assert
            var taskResult = Assert.IsType<OkObjectResult>(task);
            var objectResult = Assert.IsType<CartClientOutputDto>(taskResult.Value);
            Assert.Empty(objectResult.Items);
            Assert.Equal(0, objectResult.AllValue);
            _mockedCartClientRepository.Verify(repository => repository.UpdateAsync(It.IsAny<CartClient>()), Times.Exactly(1));
        }

        [Fact]
        public async Task RemoveCartItemFromCartClientAsync_Throws_CorrectException()
        {
            // Arrange
            var cart = new CartClient(null, 0);
            _mockedCartClientRepository
                .Setup(repository => repository.GetByCartIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cart);
            var controller = CreateCartController();

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.RemoveCartItemFromCartClientAsync(Guid.NewGuid(), Guid.NewGuid()));
            _mockedCartClientRepository.Verify(repository => repository.UpdateAsync(It.IsAny<CartClient>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCart_Returns_204NoContent()
        {
            // Arrange
            var c = new CartClient(null, 0);
            _mockedCartClientRepository
                .Setup(repository => repository.GetByCartIdAsync(c.Id))
                .ReturnsAsync(() => c);
            var controller = CreateCartController();

            // Act
            var task = await controller.DeleteCart(c.Id);

            // Assert
            Assert.IsType<NoContentResult>(task);
            _mockedCartClientRepository.Verify(repository => repository.RemoveAsync(It.IsAny<CartClient>()), Times.Exactly(1));
        }

        [Fact]
        public async Task DeleteCart_Throws_CorrectException()
        {
            // Arrange
            _mockedCartClientRepository
                .Setup(repository => repository.GetByCartIdAsync(Guid.NewGuid()))
                .ReturnsAsync(() => null);
            var controller = CreateCartController();

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.DeleteCart(Guid.NewGuid()));
            _mockedCartClientRepository.Verify(repository => repository.RemoveAsync(It.IsAny<CartClient>()), Times.Never);
        }

        private CartController CreateCartController()
        {
            var productAppService = new ProductAppService(_mockedProductRepository.Object, new Mock<ILogger<ProductAppService>>().Object, _mapper);
            var clientAppService = new ClientAppService(_mockedClientRepository.Object, new Mock<ILogger<ClientAppService>>().Object, _mapper);

            var cartAppService = new CartAppService(_mockedCartClientRepository.Object, productAppService, clientAppService, _mockedLogger.Object, _mapper);
            return new CartController(cartAppService, clientAppService);
        }
    }
}
