using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.Repositories.Cart
{
    public class CartClientRepository : RepositoryBase<CartClient>, ICartClientRepository
    {
        public CartClientRepository(DataContext context) : base(context) { }

        public Task<CartClient?> GetByCartIdAsync(Guid id)
        {
            return _dbSet
                .Include(o => o.Client)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<CartClient?> GetByClientIdAsync(Guid clientId)
        {
            return await _dbSet
                .Include(o => o.Client)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.ClientId == clientId);
        }

        public override async Task UpdateAsync(CartClient obj)
        {
            var original = await _db.Set<CartClient>()
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
    }
}
