using System.ComponentModel;

namespace Fiap.McTech.Domain.Enums
{
	public enum PaymentStatus
	{
		[Description("Nenhum")]
		None = -1,

		[Description("Pendente")]
		Pending,

		[Description("Completo")]
		Completed,

		[Description("Cancelado")]
		Canceled,

		[Description("Falhou")]
		Failed
	}
}