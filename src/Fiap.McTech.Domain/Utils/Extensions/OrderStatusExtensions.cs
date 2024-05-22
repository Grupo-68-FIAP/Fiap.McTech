using Fiap.McTech.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.McTech.Domain.Utils.Extensions
{
    public static class OrderStatusExtensions
    {
        public static OrderStatus NextStatus(this OrderStatus currentStatus)
        {
            switch (currentStatus)
            {
                case OrderStatus.None:
                    return OrderStatus.Pending;
                case OrderStatus.Pending:
                    return OrderStatus.Processing;
                case OrderStatus.Processing: 
                    return OrderStatus.Completed;
                case OrderStatus.Completed: 
                    return OrderStatus.Completed;
                case OrderStatus.Canceled: 
                    return OrderStatus.Canceled;
                default: 
                    return OrderStatus.None;
            }
        }
    }
}
