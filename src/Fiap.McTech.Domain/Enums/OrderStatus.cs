using System.ComponentModel;

namespace Fiap.McTech.Domain.Enums
{
	public enum OrderStatus
	{
		[Description("Nenhum")]
		None = -1,

		[Description("Pendente")]
		Pending,

		[Description("Processando")]
		Processing,

		[Description("Completo")]
		Completed,

		[Description("Cancelado")]
		Canceled
	}
}