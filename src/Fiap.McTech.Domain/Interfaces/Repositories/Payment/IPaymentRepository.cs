using Fiap.McTech.Domain.Entities.Payments;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Payments
{
    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
        Task<Payment?> GetByOrderIdAsync(Guid id);
    }
}