using Fiap.McTech.Domain.Entities;
using Fiap.McTech.Domain.Interfaces.Repositories;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.UnitTests.Repositories
{
    public abstract class RepositoryBaseUnitTests<TEntity> : IDisposable where TEntity : EntityBase
    {
        protected readonly DataContext _context;
        protected readonly TEntity _entity;

        protected RepositoryBaseUnitTests(TEntity entity)
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new DataContext(options);
            _entity = entity;
        }

        protected abstract IRepositoryBase<TEntity> GetRepository();

        [Fact]
        public async Task RemoveAsync_Ok()
        {
            // Arrange
            using var repository = GetRepository();

            // Act
            var result1 = await repository.GetByIdAsync(_entity.Id);
            await repository.RemoveAsync(_entity);
            var result2 = await repository.GetByIdAsync(_entity.Id);

            // Assert
            Assert.NotNull(result1);
            Assert.Null(result2);
        }

        [Fact]
        public async Task GetAll_Returns_Any()
        {
            // Arrange
            using var repository = GetRepository();

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Entity()
        {
            // Arrange
            using var repository = GetRepository();

            // Act
            var result = await repository.GetByIdAsync(_entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(_entity.Equals(result));

        }

        [Fact]
        public void GetById_Returns_Entity()
        {
            // Arrange
            using var repository = GetRepository();

            // Act
            var result = repository.GetById(_entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(_entity.Equals(result));

        }

        [Fact]
        public async Task UpdateAsync_Ok()
        {
            // Arrange
            var updated = _entity.UpdatedDate;
            using var repository = GetRepository();
            _entity.UpdatedDate = DateTime.Now;

            // Act
            await repository.UpdateAsync(_entity);
            var n = await repository.GetByIdAsync(_entity.Id);

            // Assert
            Assert.False(updated.Equals(n?.UpdatedDate));
        }

        [Fact]
        public void Update_Ok()
        {
            // Arrange
            using var repository = GetRepository();
            var updated = _entity.UpdatedDate;
            _entity.UpdatedDate = DateTime.Now;

            // Act
            repository.Update(_entity);
            var n = repository.GetById(_entity.Id);

            // Assert
            Assert.False(updated.Equals(n?.UpdatedDate));
        }

        protected abstract TEntity GetEntityToAddTest();

        [Fact]
        public async Task AddAsync_Returns_Entity()
        {
            // Arrange
            using var repository = GetRepository();
            var entity = GetEntityToAddTest();

            // Act
            var result = await repository.AddAsync(entity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity, result);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _context?.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RepositoryBaseUnitTests()
        {
            Dispose(false);
        }
    }
}
