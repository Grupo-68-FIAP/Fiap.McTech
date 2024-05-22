using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.Repositories.Orders
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(context) { }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(o => o.Client)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
            ;
        }

        public async Task<List<Order>> GetOrderByStatusAsync(OrderStatus status)
        {
            return await _dbSet
                .Include(o => o.Client)
                .Include(o => o.Items)
                .Where(o => o.Status == status)
                .ToListAsync();
            ;
        }
    }
}