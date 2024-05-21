using Fiap.McTech.Domain.Entities.Cart;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Cart
{
	public interface ICartClientRepository : IRepositoryBase<CartClient>
	{
		Task<CartClient?> GetByCartIdAsync(Guid clientId);
		Task<CartClient?> GetByClientIdAsync(Guid clientId);
    }
}