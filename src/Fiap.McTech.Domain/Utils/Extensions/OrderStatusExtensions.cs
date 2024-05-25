using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Utils.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="OrderStatus"/> enumeration.
    /// </summary>
    public static class OrderStatusExtensions
    {
        /// <summary>
        /// Gets the next status for the order based on the current status.
        /// </summary>
        /// <param name="currentStatus">The current status of the order.</param>
        /// <returns>The next status for the order.</returns>
        public static OrderStatus NextStatus(this OrderStatus currentStatus)
        {
            return currentStatus switch
            {
                OrderStatus.None => OrderStatus.Pending,
                OrderStatus.Pending => OrderStatus.Processing,
                OrderStatus.Processing => OrderStatus.Completed,
                OrderStatus.Completed => OrderStatus.Completed,
                OrderStatus.Canceled => OrderStatus.Canceled,
                _ => OrderStatus.None,
            };
        }
    }
}
