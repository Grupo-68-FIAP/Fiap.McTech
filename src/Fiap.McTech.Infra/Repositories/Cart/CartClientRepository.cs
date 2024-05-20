using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.Repositories.Cart
{
    public class CartClientRepository : RepositoryBase<CartClient>, ICartClientRepository
    {
        public CartClientRepository(DataContext context) : base(context) { }

        public Task<CartClient?> GetByCartIdAsync(Guid uuid)
        {
            return _dbSet
                .Include(o => o.Client)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == uuid);
        }

        public async Task<CartClient?> GetByClientIdAsync(Guid clientId)
        {
            return await _dbSet
                .Include(o => o.Client)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.ClientId == clientId);
        }
    }
}