using Fiap.McTech.Domain.Entities.Cart; 
using Fiap.McTech.Domain.Interfaces.Repositories.Cart; 
using Fiap.McTech.Infra.Context;

namespace Fiap.McTech.Infra.Repositories.Cart
{
	public class CartClientRepository : RepositoryBase<CartClient>, ICartClientRepository
	{
		public CartClientRepository(DataContext context) : base(context) { }
	}
}