using AutoMapper;
using Fiap.McTech.Api.Controllers.Product;
using Fiap.McTech.Application.AppServices.Product;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Products.Add;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.MacTech.Api.UnitTests.Controllers
{
    public class ProducControllerUnitTests
    {
        readonly Mock<IProductRepository> _mockedProductRepository;
        readonly Mock<ILogger<ProductAppService>> _mockedLogger;
        readonly IMapper _mapper;

        readonly List<Product> _productListForTest = new()
        {
            new ("Test 1", 10, "Product test 1", "", ProductCategory.None),
            new ("Test 2", 20, "Product test 2", "", ProductCategory.Snack)
        };

        public ProducControllerUnitTests()
        {
            _mockedProductRepository = new Mock<IProductRepository>();
            _mockedLogger = new Mock<ILogger<ProductAppService>>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>());
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public void GetAllProducts_Returns_204NoContent()
        {
            // Arrange
            _mockedProductRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(() => new List<Product>());
            var controller = GetProductController();

            // Act
            var result = controller.GetAllProducts().Result;

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void GetAllProducts_Returns_200Ok()
        {
            // Arrange
            _mockedProductRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(() => _productListForTest);
            var controller = GetProductController();

            // Act
            var result = controller.GetAllProducts().Result;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var listResult = ((OkObjectResult) result).Value as IEnumerable<dynamic>;
            Assert.Equal(_productListForTest.Count, listResult?.Count());
            Assert.True(listResult?.Any(c => c.Name == _productListForTest[0].Name));
            Assert.True(listResult?.Any(c => c.Name == _productListForTest[1].Name));
        }

        [Fact]
        public void GetProduct_Returns_200OK()
        {
            // Arrange
            var p = _productListForTest[0];
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(p.Id))
                .ReturnsAsync(() => p);
            var controller = GetProductController();

            // Act
            var task = controller.GetProduct(p.Id).Result;

            // Assert
            Assert.IsType<OkObjectResult>(task);

            var taskResult = task as OkObjectResult;
            Assert.IsType<ProductOutputDto>(taskResult?.Value);

            var objectResult = taskResult.Value as dynamic;
            Assert.Equal(p.Name, objectResult?.Name);
            Assert.Equal(p.Id, objectResult?.Id);
        }

        [Fact]
        public void GetProduct_Throws_EntityNotFoundException()
        {
            // Arrange
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(Guid.NewGuid()))
                .ReturnsAsync(() => null);
            var controller = GetProductController();
            var guidSearch = Guid.NewGuid();

            // Act & Assert
            var task = Assert.ThrowsAsync<EntityNotFoundException>(() => controller.GetProduct(guidSearch)).Result;
            Assert.Contains(guidSearch.ToString(), task.Message);
        }

        [Fact]
        public void CreateProduct_Returns_201Created()
        {
            // Arrange
            _mockedProductRepository
                .Setup(repository => repository.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync<Product, IProductRepository, Product>(x => x);
            var controller = GetProductController();
            var input = new CreateProductInputDto("Test 1", 10, "Product test 1", ProductCategory.Snack);

            // Act
            var result = controller.CreateProduct(input).Result;

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            _mockedProductRepository.Verify(repository => repository.AddAsync(It.IsAny<Product>()), Times.Exactly(1));
        }

        [Fact]
        public void CreateProduct_Throws_CorrectException()
        {
            // Arrange
            var input = new CreateProductInputDto("", 0, "", ProductCategory.Snack);

            var controller = GetProductController();

            // Act & Assert
            var task = Assert.ThrowsAsync<EntityValidationException>(() => controller.CreateProduct(input)).Result;
            Assert.Contains("invalid data", task.Message);
            _mockedProductRepository.Verify(repository => repository.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void UpdateProduct_Returns_200OK()
        {
            // Arrange
            var p = _productListForTest[0];
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(p.Id))
                .ReturnsAsync(p);
            _mockedProductRepository
                .Setup(repository => repository.UpdateAsync(p));
            var controller = GetProductController();
            var input = new UpdateProductInputDto() { Id = p.Id, Category = p.Category, Description = p.Description, Name = p.Name, Value = p.Value };

            // Act
            var result = controller.UpdateProduct(p.Id, input).Result;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockedProductRepository.Verify(repository => repository.UpdateAsync(p), Times.Exactly(1));
        }

        [Fact]
        public void UpdateProduct_Throws_EntityNotFoundException()
        {
            // Arrange
            var p = _productListForTest[0];
            var guid = Guid.NewGuid();
            var input = new UpdateProductInputDto() { Id = guid, Category = p.Category, Description = p.Description, Name = p.Name, Value = p.Value };
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(guid))
                .ReturnsAsync(() => null);
            var controller = GetProductController();

            // Act & Assert
            var task = Assert.ThrowsAsync<EntityNotFoundException>(() => controller.UpdateProduct(guid, input)).Result;
            Assert.Contains(guid.ToString(), task.Message);
            _mockedProductRepository.Verify(repository => repository.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void UpdateProduct_Throws_EntityValidationException()
        {
            // Arrange
            var p = _productListForTest[0];
            var input = new UpdateProductInputDto() { Id = p.Id };
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(p.Id))
                .ReturnsAsync(() => p);
            var controller = GetProductController();

            // Act & Assert
            var task = Assert.ThrowsAsync<EntityValidationException>(() => controller.UpdateProduct(p.Id, input)).Result;
            Assert.Contains("invalid data", task.Message);
            _mockedProductRepository.Verify(repository => repository.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void DeleteClient_Returns_204NoContent()
        {
            // Arrange
            var p = _productListForTest[0];
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(p.Id))
                .ReturnsAsync(() => p);
            var controller = GetProductController();

            // Act
            var result = controller.DeleteProduct(p.Id).Result;

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockedProductRepository.Verify(repository => repository.RemoveAsync(p), Times.Exactly(1));
        }

        [Fact]
        public void DeleteClient_Throws_EntityNotFoundException()
        {
            // Arrange
            var guid = Guid.NewGuid();
            _mockedProductRepository
                .Setup(repository => repository.GetByIdAsync(guid))
                .ReturnsAsync(() => null);
            var controller = GetProductController();

            // Act & Assert
            var task = Assert.ThrowsAsync<EntityNotFoundException>(() => controller.DeleteProduct(guid)).Result;
            Assert.Contains(guid.ToString(), task.Message);
            _mockedProductRepository.Verify(repository => repository.RemoveAsync(It.IsAny<Product>()), Times.Never);
        }

        private ProductController GetProductController()
        {
            var appService = new ProductAppService(_mockedProductRepository.Object, _mockedLogger.Object, _mapper);
            return new ProductController(appService);
        }
    }
}
