using System.ComponentModel;

namespace Fiap.McTech.Domain.Enums
{
    /// <summary>
    /// Represents the status of a payment.
    /// </summary>
	public enum PaymentStatus
    {
        /// <summary>
        /// No status.
        /// </summary>
		[Description("Nenhum")]
        None = -1,

        /// <summary>
        /// The payment is pending.
        /// </summary>
        [Description("Pendente")]
        Pending,

        /// <summary>
        /// The payment is completed.
        /// </summary>
        [Description("Completo")]
        Completed,

        /// <summary>
        /// The payment is canceled.
        /// </summary>
        [Description("Cancelado")]
        Canceled,

        /// <summary>
        /// The payment failed.
        /// </summary>
        [Description("Falhou")]
        Failed
    }
}
