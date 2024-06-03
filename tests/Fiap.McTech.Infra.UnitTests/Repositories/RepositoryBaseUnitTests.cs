using Fiap.McTech.Domain.Entities;
using Fiap.McTech.Domain.Interfaces.Repositories;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.UnitTests.Repositories
{
    public abstract class RepositoryBaseUnitTests<TEntity> : IDisposable where TEntity : EntityBase
    {
        protected readonly DbContextOptions<DataContext> _options;
        protected DataContext? _context;

        protected RepositoryBaseUnitTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        }

        protected abstract IRepositoryBase<TEntity> GetRepository(DataContext context);

        protected abstract TEntity MakeNewEntity();

        protected void Before()
        {
            _context = new DataContext(_options);
        }

        protected void After()
        {
            _context?.Dispose();
        }

        [Fact]
        public async Task GetAll_Returns_Noting()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Any());

            After();
        }

        [Fact]
        public async Task GetAll_Returns_Any()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            var dbSet = _context.Set<TEntity>();
            await dbSet.AddAsync(MakeNewEntity());
            await dbSet.AddAsync(MakeNewEntity());
            await dbSet.AddAsync(MakeNewEntity());
            await _context.SaveChangesAsync();

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(3, result.Count());

            After();
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Entity()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            var dbSet = _context.Set<TEntity>();
            var _entity = MakeNewEntity();
            await dbSet.AddAsync(_entity);
            await _context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(_entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(_entity.Equals(result));

            After();
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Null()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            // Act
            var result = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);

            After();
        }

        [Fact]
        public void GetById_Returns_Entity()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            var dbSet = _context.Set<TEntity>();
            var _entity = MakeNewEntity();
            dbSet.Add(_entity);
            _context.SaveChanges();

            // Act
            var result = repository.GetById(_entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(_entity.Equals(result));

            After();
        }

        [Fact]
        public void GetById_Returns_Null()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            // Act
            var result = repository.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(result);

            After();
        }

        [Fact]
        public async Task RemoveAsync_Ok()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            var dbSet = _context.Set<TEntity>();
            var _entityToRemove = MakeNewEntity();
            await dbSet.AddAsync(_entityToRemove);
            await _context.SaveChangesAsync();

            // Act
            await repository.RemoveAsync(_entityToRemove);
            var result = await repository.GetByIdAsync(_entityToRemove.Id);

            // Assert
            Assert.Null(result);

            After();
        }

        [Fact]
        public void Remove_Ok()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            var dbSet = _context.Set<TEntity>();
            var _entityToRemove = MakeNewEntity();
            dbSet.Add(_entityToRemove);
            _context.SaveChanges();

            // Act
            repository.Remove(_entityToRemove);
            var result = repository.GetById(_entityToRemove.Id);

            // Assert
            Assert.Null(result);

            After();
        }

        [Fact]
        public async Task RemoveAsync_Throws_NotFound()
        {
            try
            {
                Before();
                if (_context == null)
                    Assert.Fail();

                // Arrange
                using var repository = GetRepository(_context);

                // Act & Assert
                await Assert.ThrowsAnyAsync<Exception>(() => repository.RemoveAsync(MakeNewEntity()));
            }
            finally
            {
                After();
            }
        }

        [Fact]
        public void Remove_Throws_NotFound()
        {
            try
            {
                Before();
                if (_context == null)
                    Assert.Fail();

                // Arrange
                using var repository = GetRepository(_context);

                // Act & Assert
                Assert.ThrowsAny<Exception>(() => repository.Remove(MakeNewEntity()));
            }
            finally
            {
                After();
            }
        }

        [Fact]
        public async Task UpdateAsync_Ok()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            var dbSet = _context.Set<TEntity>();
            var _entity = MakeNewEntity();
            await dbSet.AddAsync(_entity);
            await _context.SaveChangesAsync();
            var updated = _entity.UpdatedDate;
            _entity.UpdatedDate = DateTime.Now;

            // Act
            await repository.UpdateAsync(_entity);
            var n = await repository.GetByIdAsync(_entity.Id);

            // Assert
            Assert.NotEqual(updated, n?.UpdatedDate);

            After();
        }

        [Fact]
        public void Update_Ok()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);

            var dbSet = _context.Set<TEntity>();
            var _entity = MakeNewEntity();
            dbSet.Add(_entity);
            _context.SaveChanges();

            var updated = _entity.UpdatedDate;
            _entity.UpdatedDate = DateTime.Now;

            // Act
            repository.Update(_entity);
            var n = repository.GetById(_entity.Id);

            // Assert
            Assert.NotEqual(updated, n?.UpdatedDate);

            After();
        }

        [Fact]
        public async Task AddAsync_Returns_Entity()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);
            var _entity = MakeNewEntity();

            // Act
            var result = await repository.AddAsync(_entity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_entity, result);

            After();
        }

        [Fact]
        public void Add_Returns_Entity()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);
            var _entity = MakeNewEntity();

            // Act
            var result = repository.Add(_entity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_entity, result);

            After();
        }

        [Fact]
        public async Task AddAsync_Throws_NotFound()
        {
            try
            {
                Before();
                if (_context == null)
                    Assert.Fail();

                // Arrange
                using var repository = GetRepository(_context);
                var _entity = MakeNewEntity();
                var dbSet = _context.Set<TEntity>();
                await dbSet.AddAsync(_entity);
                await _context.SaveChangesAsync();

                // Act & Assert
                await Assert.ThrowsAnyAsync<Exception>(() => repository.AddAsync(_entity));
            }
            finally
            {
                After();
            }
        }

        [Fact]
        public void Add_Throws_NotFound()
        {
            try
            {
                Before();
                if (_context == null)
                    Assert.Fail();

                // Arrange
                using var repository = GetRepository(_context);
                var _entity = MakeNewEntity();
                var dbSet = _context.Set<TEntity>();
                dbSet.Add(_entity);
                _context.SaveChanges();

                // Act & Assert
                Assert.ThrowsAny<Exception>(() => repository.Add(_entity));
            }
            finally
            {
                After();
            }
        }

        [Fact]
        public async Task AddRangeAsync_Returns_List()
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            using var repository = GetRepository(_context);
            var _entities = new List<TEntity> { MakeNewEntity(), MakeNewEntity(), MakeNewEntity() };

            // Act
            var result = await repository.AddRangeAsync(_entities);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_entities, result);

            After();
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
