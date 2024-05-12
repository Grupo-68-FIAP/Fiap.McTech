using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Infra.Context; 

namespace Fiap.McTech.Infra.Repositories.Orders
{
	public class OrderRepository : RepositoryBase<Order>, IOrderRepository
	{
		public OrderRepository(DataContext context) : base(context) { }
	}
}