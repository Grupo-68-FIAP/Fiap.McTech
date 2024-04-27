using System;

namespace Fiap.McTech.Domain.Interfaces.Repositories
{
	public interface IRepositoryBase<TEntity> where TEntity : class
	{
		void Add(TEntity obj);
		Task<TEntity> AddAsync(TEntity obj);
		Task<List<TEntity>> AddRangeAsync(List<TEntity> obj);
		TEntity GetById(Guid id);
		Task<TEntity> GetByIdAsync(Guid id);
		Task<TEntity> GetByCrmAsync(string crm);
		IEnumerable<TEntity> GetAll();
		void Update(TEntity obj);
		Task UpdateAsync(TEntity obj);
		void Remove(TEntity obj);
		Task RemoveAsync(TEntity obj);
		void Dispose();
	}
}