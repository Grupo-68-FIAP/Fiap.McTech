using System.ComponentModel;

namespace Fiap.McTech.Domain.Enums
{
    /// <summary>
    /// Represents the status of an order:<br></br>
    /// <list type="bullet">
    /// <item><description><c>Canceled</c> (-2): Orders that are canceled.</description></item>
    /// <item><description><c>None</c> (-1): No specific status.</description></item>
    /// <item><description><c>WaitPayment</c> (0): Orders that are waiting for payment.</description></item>
    /// <item><description><c>Received</c> (1): Orders that have been received.</description></item>
    /// <item><description><c>InPreparation</c> (2): Orders that are being prepared.</description></item>
    /// <item><description><c>Ready</c> (3): Orders that are ready.</description></item>
    /// <item><description><c>Finished</c> (4): Orders that are completed and delivered.</description></item>
    /// </list>
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
        [Description("Aguardando Pagamento")]
        WaitPayment,

        /// <summary>
        /// The order is received.
        /// </summary>
        [Description("Recebido")]
        Received,

        /// <summary>
        /// The order is being prepared.
        /// </summary>
        [Description("Em Preparo")]
        InPreparation,

        /// <summary>
        /// The order is ready.
        /// </summary>
        [Description("Pronto")]
        Ready,

        /// <summary>
        /// The order is completed and delivered
        /// </summary>
        [Description("Finalizado")]
        Finished,

        /// <summary>
        /// The order is canceled.
        /// </summary>
        [Description("Cancelado")]
        Canceled = -2
    }
}
