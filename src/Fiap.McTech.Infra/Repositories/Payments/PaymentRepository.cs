using Fiap.McTech.Domain.Entities.Payments;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Fiap.McTech.Domain.ValuesObjects;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.Repositories.Payments
{
	public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
	{
		public PaymentRepository(DataContext context) : base(context) { }

        public async Task<Payment?> GetByOrderIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}