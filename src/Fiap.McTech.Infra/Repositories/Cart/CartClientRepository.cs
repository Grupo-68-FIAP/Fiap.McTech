using Fiap.McTech.Domain.Entities.Cart; 
using Fiap.McTech.Domain.Interfaces.Repositories.Cart; 
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.Repositories.Cart
{
	public class CartClientRepository : RepositoryBase<CartClient>, ICartClientRepository
	{
		public CartClientRepository(DataContext context) : base(context) { }

		public async Task<CartClient?> GetByClientIdAsync(Guid clientId) {
			return await _dbSet.Where(cart => clientId.Equals(cart.ClientId)).FirstOrDefaultAsync();
		}
	}
}