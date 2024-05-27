using Fiap.McTech.Domain.Entities.Payments;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Payments
{
    /// <summary>
    /// Represents a repository interface for CRUD operations with payments in the McTech domain.
    /// </summary>
    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
        /// <summary>
        /// Asynchronously retrieves a payment by its associated order identifier.
        /// </summary>
        /// <param name="id">The identifier of the order associated with the payment.</param>
        /// <returns>A task representing the asynchronous operation, containing the payment associated with the specified order identifier, if found; otherwise, null.</returns>
        Task<Payment?> GetByOrderIdAsync(Guid id);
    }
}
