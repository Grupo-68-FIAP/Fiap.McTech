using AutoMapper;
using Fiap.McTech.Api.Controllers.Clients;
using Fiap.McTech.Application.AppServices.Clients;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Fiap.McTech.Domain.ValuesObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.MacTech.Api.UnitTests.Controllers
{
    public class ClientControllerUnitTests
    {
        readonly Mock<IClientRepository> _mockedClientRepository;
        readonly Mock<ILogger<ClientAppService>> _mockedLogger;
        readonly IMapper _mapper;

        public ClientControllerUnitTests()
        {
            _mockedClientRepository = new Mock<IClientRepository>();
            _mockedLogger = new Mock<ILogger<ClientAppService>>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ClientProfile>());
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public async Task GetAllClients_Returns_204NoContent()
        {
            // Arrange
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetAll())
                .ReturnsAsync(() => new List<Client>());
            var controller = GetClientController();

            // Act
            var result = await controller.GetAllClients();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAllClients_Returns_200Ok()
        {
            // Arrange
            var c1 = new Client("Test 1", new("72518830073"), new("test1@test.com"));
            var c2 = new Client("Test 2", new("49380966091"), new("test2@test.com"));
            var list = new List<Client> { c1, c2 };
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetAll())
                .ReturnsAsync(() => list);
            var controller = GetClientController();

            // Act
            var result = await controller.GetAllClients();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var listResult = ((OkObjectResult) result).Value as IEnumerable<dynamic>;
            Assert.Equal(list.Count, listResult?.Count());
            Assert.True(listResult?.Any(c => c.Name == c1.Name));
            Assert.True(listResult?.Any(c => c.Name == c2.Name));
        }

        [Fact]
        public async Task GetClient_Returns_200OK()
        {
            // Arrange
            var c = new Client("Test 1", new("72518830073"), new("test1@test.com"));
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetByIdAsync(c.Id))
                .ReturnsAsync(() => c);
            var controller = GetClientController();

            // Act
            var task = await controller.GetClient(c.Id);

            // Assert
            Assert.IsType<OkObjectResult>(task);
            var taskResult = task as OkObjectResult;
            Assert.IsType<ClientOutputDto>(taskResult?.Value);
            var objectResult = taskResult.Value as dynamic;
            Assert.Equal(c.Id, objectResult?.Id);
            Assert.Equal(c.Name, objectResult?.Name);
        }

        [Fact]
        public async Task GetClient_Throws_EntityNotFoundException()
        {
            // Arrange
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetByIdAsync(Guid.NewGuid()))
                .ReturnsAsync(() => null);
            var controller = GetClientController();
            var guidSearch = Guid.NewGuid();

            // Act & Assert
            var task = await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.GetClient(guidSearch));
            Assert.Contains(guidSearch.ToString(), task.Message);
        }

        [Fact]
        public async Task GetClientByCpf_Returns_200OK()
        {
            // Arrange
            var cpf = "49380966091";
            var c = new Client("Test 2", new(cpf), new("test2@test.com"));
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetClientAsync(c.Cpf))
                .ReturnsAsync(() => c);
            var controller = GetClientController();

            // Act
            var task = await controller.GetClientByCpf(cpf);

            // Assert
            Assert.IsType<OkObjectResult>(task);
            var taskResult = task as OkObjectResult;
            Assert.IsType<ClientOutputDto>(taskResult?.Value);
            var objectResult = taskResult.Value as dynamic;
            Assert.Equal(c.Id, objectResult?.Id);
            Assert.Equal(c.Name, objectResult?.Name);
        }

        [Theory]
        [InlineData("00011122233", typeof(EntityValidationException))]
        [InlineData("07059775951", typeof(EntityNotFoundException))]
        public async Task GetClientByCpf_Throws_CorrectException(string search, Type expectedExceptionType)
        {
            // Arrange
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetClientAsync(It.IsAny<Cpf>()))
                .ReturnsAsync(() => null);
            var controller = GetClientController();

            // Act & Assert
            var task = await Assert.ThrowsAsync(expectedExceptionType, () => controller.GetClientByCpf(search));
            Assert.Contains(search, task.Message);
        }


        [Fact]
        public async Task GetClientByEmail_Returns_200OK()
        {
            // Arrange
            var email = "test2@test.com";
            var c = new Client("Test 2", new("49380966091"), new(email));
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetClientAsync(c.Email))
                .ReturnsAsync(() => c);
            var controller = GetClientController();

            // Act
            var task = await controller.GetClientByEmail(email);

            // Assert
            Assert.IsType<OkObjectResult>(task);

            var taskResult = task as OkObjectResult;
            Assert.IsType<ClientOutputDto>(taskResult?.Value);

            var objectResult = taskResult.Value as dynamic;
            Assert.Equal(c.Name, objectResult?.Name);
            Assert.Equal(c.Id, objectResult?.Id);
        }

        [Theory]
        [InlineData("invalid_email", typeof(EntityValidationException))]
        [InlineData("test@test.com", typeof(EntityNotFoundException))]
        public async Task GetClientByEmail_Throws_CorrectException(string search, Type expectedExceptionType)
        {
            // Arrange
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetClientAsync(It.IsAny<Email>()))
                .ReturnsAsync(() => null);
            var controller = GetClientController();

            // Act & Assert
            var task = await Assert.ThrowsAsync(expectedExceptionType, () => controller.GetClientByEmail(search));
            Assert.Contains(search, task.Message);
        }

        [Fact]
        public async Task CreateClient_Returns_201Created()
        {
            // Arrange
            _mockedClientRepository
                .Setup(clientRepository => clientRepository.AddAsync(It.IsAny<Client>()))
                .ReturnsAsync<Client, IClientRepository, Client>(x => x);
            var controller = GetClientController();
            var input = new ClientInputDto("Test1", "72518830073", "test1@test.com");

            // Act
            var result = await controller.CreateClient(input);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            _mockedClientRepository.Verify(clientRepository => clientRepository.AddAsync(It.IsAny<Client>()), Times.Exactly(1));
        }

        public static TheoryData<ClientInputDto, Type, string> CreateClient_Parameters()
        {
            return new TheoryData<ClientInputDto, Type, string>
            {
                { new ClientInputDto("Test", "94682236040", "test3@test.com"), typeof(EntityValidationException), "test3@test.com" },
                { new ClientInputDto("Test", "72518830073", "test@test.com"), typeof(EntityValidationException), "72518830073" },
                { new ClientInputDto("", "94682236040", "test@test.com"), typeof(EntityValidationException), "invalid data" },
            };
        }

        [Theory]
        [MemberData(nameof(CreateClient_Parameters))]
        public async Task CreateClient_Throws_CorrectException(ClientInputDto input, Type expectedExceptionType, string msgPart)
        {
            // Arrange
            var c = new Client("Test 3", new("72518830073"), new("test3@test.com"));
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetClientAsync(c.Cpf))
                .ReturnsAsync(() => c);
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetClientAsync(c.Email))
                .ReturnsAsync(() => c);
            _mockedClientRepository
                .Setup(clientRepository => clientRepository.AddAsync(It.IsAny<Client>()))
                .ReturnsAsync<Client, IClientRepository, Client>(x => x);
            var controller = GetClientController();

            // Act & Assert
            var task = await Assert.ThrowsAsync(expectedExceptionType, () => controller.CreateClient(input));
            Assert.Contains(msgPart, task.Message);
            _mockedClientRepository.Verify(clientRepository => clientRepository.AddAsync(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public async Task UpdateClient_Returns_200OK()
        {
            // Arrange
            var c = new Client("Test 1", new("72518830073"), new("test1@test.com"));
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetByIdAsync(c.Id))
                .ReturnsAsync(() => c);
            _mockedClientRepository
                .Setup(clientRepository => clientRepository.UpdateAsync(c));
            var controller = GetClientController();
            var input = new ClientInputDto("Test 1", "72518830073", "test1@test.com");

            // Act
            var result = await controller.UpdateClient(c.Id, input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockedClientRepository.Verify(clientRepository => clientRepository.UpdateAsync(c), Times.Exactly(1));
        }

        [Fact]
        public async Task UpdateClient_Throws_EntityNotFoundException()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var inputDto = new ClientInputDto("Test", "94682236040", "test3@test.com");
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetByIdAsync(guid))
                .ReturnsAsync(() => null);
            var controller = GetClientController();

            // Act & Assert
            var task = await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.UpdateClient(guid, inputDto));
            Assert.Contains(guid.ToString(), task.Message);
            _mockedClientRepository.Verify(clientRepository => clientRepository.UpdateAsync(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public async Task UpdateClient_Throws_EntityValidationException()
        {
            // Arrange
            var inputDto = new ClientInputDto("", "72518830073", "test3@test.com");
            var c = new Client("Test 3", new("72518830073"), new("test3@test.com"));
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetByIdAsync(c.Id))
                .ReturnsAsync(() => c);
            var controller = GetClientController();

            // Act & Assert
            var task = await Assert.ThrowsAsync<EntityValidationException>(() => controller.UpdateClient(c.Id, inputDto));
            Assert.Contains("invalid data", task.Message);
            _mockedClientRepository.Verify(clientRepository => clientRepository.UpdateAsync(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public async Task DeleteClient_Returns_204NoContent()
        {
            // Arrange
            var c = new Client("Test 3", new("72518830073"), new("test3@test.com"));
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetByIdAsync(c.Id))
                .ReturnsAsync(() => c);
            var controller = GetClientController();

            // Act
            var result = await controller.DeleteClient(c.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockedClientRepository.Verify(clientRepository => clientRepository.RemoveAsync(c), Times.Exactly(1));
        }

        [Fact]
        public async Task DeleteClient_Throws_EntityNotFoundException()
        {
            // Arrange
            var guid = Guid.NewGuid();
            _mockedClientRepository
                .Setup(iClientRepository => iClientRepository.GetByIdAsync(guid))
                .ReturnsAsync(() => null);
            var controller = GetClientController();

            // Act & Assert
            var task = await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.DeleteClient(guid));
            Assert.Contains(guid.ToString(), task.Message);
            _mockedClientRepository.Verify(clientRepository => clientRepository.RemoveAsync(It.IsAny<Client>()), Times.Never);
        }

        private ClientController GetClientController()
        {
            var clientAppService = new ClientAppService(_mockedClientRepository.Object, _mockedLogger.Object, _mapper);
            return new ClientController(clientAppService);
        }
    }
}
