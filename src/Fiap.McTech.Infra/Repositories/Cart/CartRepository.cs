using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;

namespace Fiap.McTech.Infra.Repositories.Cart
{
	public class CartRepository : ICartRepository
	{
		public void Add(CartClient obj)
		{
			throw new NotImplementedException();
		}

		public Task<CartClient> AddAsync(CartClient obj)
		{
			throw new NotImplementedException();
		}

		public Task<List<CartClient>> AddRangeAsync(List<CartClient> obj)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CartClient> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task<CartClient> GetByCrmAsync(string crm)
		{
			throw new NotImplementedException();
		}

		public CartClient GetById(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<CartClient> GetByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public void Remove(CartClient obj)
		{
			throw new NotImplementedException();
		}

		public Task RemoveAsync(CartClient obj)
		{
			throw new NotImplementedException();
		}

		public void Update(CartClient obj)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(CartClient obj)
		{
			throw new NotImplementedException();
		}
	}
}