﻿using Fiap.McTech.Domain.Entities.Orders;
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
        }

        public async Task<List<Order>> GetOrderByStatusAsync(OrderStatus status)
        {
            return await _dbSet
                .Include(o => o.Client)
                .Include(o => o.Items)
                .Where(o => o.Status == status)
                .ToListAsync();
        }

        public override async Task UpdateAsync(Order obj)
        {
            var original = await _db.Set<Order>()
                .AsNoTracking()
                .Include(o => o.Items)
                .FirstAsync(o => o.Id == obj.Id);

            foreach (var item in obj.Items)
            {
                if (original.Items.Any(oi => oi.Id == item.Id))
                {
                    _db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    _db.Entry(item).State = EntityState.Added;
                }
            }

            await base.UpdateAsync(obj);
        }

        public async Task<List<Order>> GetCurrrentOrders()
        {
            return await _dbSet.Include(o => o.Client).Include(o => o.Items)
                // TODO: Rever isso pois a ordem de retorno é Pronto > Em Preparo > Recebido e só temos status referente ao processo de pagamento
                .Where(o => o.Status == OrderStatus.Processing || o.Status == OrderStatus.Pending)
                .OrderBy(o => o.Status)
                .ThenByDescending(o => o.CreatedDate)
                .ToListAsync();
        }
    }
}
