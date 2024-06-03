using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Infra.Context;
using Fiap.McTech.Infra.Repositories.Clients;

namespace Fiap.McTech.Infra.UnitTests.Repositories
{
    public sealed class ClientRepositoryUnitTests : RepositoryBaseUnitTests<Client>
    {
        protected override ClientRepository GetRepository(DataContext context)
        {
            return new ClientRepository(context);
        }

        protected override Client MakeNewEntity()
        {
            Random rand = new();
            var i = rand.Next(1000, 9999);
            return new($"Client rand {i}", new("32161895036"), new($"rand{i}@test.com"));
        }

        [Fact]
        public async Task GetClientAsync_WithCpf_ShouldReturnClient()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            var repository = GetRepository(_context);

            var client = MakeNewEntity();
            var dbSet = _context.Set<Client>();
            await dbSet.AddAsync(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await repository.GetClientAsync(client.Cpf);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(client, result);

            After();
        }

        [Fact]
        public async Task GetClientAsync_WithCpf_ShouldReturnNull()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            var repository = GetRepository(_context);

            var client = MakeNewEntity();
            var dbSet = _context.Set<Client>();
            await dbSet.AddAsync(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await repository.GetClientAsync(new Domain.ValuesObjects.Cpf("12345678901"));

            // Assert
            Assert.Null(result);

            After();
        }

        [Fact]
        public async Task GetClientAsync_WithEmail_ShouldReturnClient()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            var repository = GetRepository(_context);

            var client = MakeNewEntity();
            var dbSet = _context.Set<Client>();
            await dbSet.AddAsync(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await repository.GetClientAsync(client.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(client, result);

            After();
        }

        [Fact]
        public async Task GetClientAsync_WithEmail_ShouldReturnNull()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            var repository = GetRepository(_context);

            var client = MakeNewEntity();
            var dbSet = _context.Set<Client>();
            await dbSet.AddAsync(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await repository.GetClientAsync(new Domain.ValuesObjects.Email("test@test.com"));

            // Assert
            Assert.Null(result);

            After();
        }
    }
}
