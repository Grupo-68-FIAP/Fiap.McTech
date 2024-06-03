namespace Fiap.McTech.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Represents a base repository interface for CRUD operations with entities in the McTech domain.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="obj">The entity to add.</param>
        /// <returns>The entity the specified</returns>
        TEntity Add(TEntity obj);

        /// <summary>
        /// Asynchronously adds a new entity to the repository.
        /// </summary>
        /// <param name="obj">The entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<TEntity> AddAsync(TEntity obj);

        /// <summary>
        /// Asynchronously adds a range of entities to the repository.
        /// </summary>
        /// <param name="obj">The list of entities to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<List<TEntity>> AddRangeAsync(List<TEntity> obj);

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>The entity with the specified identifier, if found; otherwise, null.</returns>
        TEntity? GetById(Guid id);

        /// <summary>
        /// Asynchronously retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task representing the asynchronous operation, containing the entity with the specified identifier if found; otherwise, null.</returns>
        Task<TEntity?> GetByIdAsync(Guid id);

        /// <summary>
        /// Asynchronously retrieves all entities from the repository.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a collection of all entities.</returns>
        Task<IEnumerable<TEntity>> GetAll();

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="obj">The entity to update.</param>
        void Update(TEntity obj);

        /// <summary>
        /// Asynchronously updates an existing entity in the repository.
        /// </summary>
        /// <param name="obj">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(TEntity obj);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <param name="obj">The entity to remove.</param>
        void Remove(TEntity obj);

        /// <summary>
        /// Asynchronously removes an entity from the repository.
        /// </summary>
        /// <param name="obj">The entity to remove.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveAsync(TEntity obj);
    }
}
