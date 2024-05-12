using Fiap.McTech.Domain.Entities.Payments;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Fiap.McTech.Infra.Context;

namespace Fiap.McTech.Infra.Repositories.Payments
{
	public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
	{
		public PaymentRepository(DataContext context) : base(context) { }
	}
}