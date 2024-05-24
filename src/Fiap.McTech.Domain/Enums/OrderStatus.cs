using System.ComponentModel;

namespace Fiap.McTech.Domain.Enums
{
    /// <summary>
    /// Represents the status of an order.
    /// </summary>
	public enum OrderStatus
    {
        /// <summary>
        /// No status.
        /// </summary>
		[Description("Nenhum")]
        None = -1,

        /// <summary>
        /// The order is pending.
        /// </summary>
        [Description("Pendente")]
        Pending,

        /// <summary>
        /// The order is being processed.
        /// </summary>
        [Description("Processando")]
        Processing,

        /// <summary>
        /// The order is completed.
        /// </summary>
        [Description("Completo")]
        Completed,

        /// <summary>
        /// The order is canceled.
        /// </summary>
        [Description("Cancelado")]
        Canceled
    }
}