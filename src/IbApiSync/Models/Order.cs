using IbApiSync.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IbApiSync.Models
{
    /// <summary>
    /// Abstract class wrapping all possible order which can be sent through IB API.
    /// </summary>
    public abstract class Order
    {
        /// <summary>
        /// Number of shares/units of this order.
        /// </summary>
        public virtual decimal Quantity { get; set; }

        /// <summary>
        /// Direction of the order.
        /// </summary>
        public virtual OrderAction Action { get; set; }

        /// <summary>
        /// Current order status. This property value can change anytime after the order was submitted.
        /// </summary>
        public OrderStatus Status { get; internal set; }

        /// <summary>
        /// Targer product of this order.
        /// </summary>
        public Product Product { get; internal set; }

        /// <summary>
        /// Average fill price for this order. This property value can change anytime after the order was submitted.
        /// </summary>
        public decimal AverageFillPrice { get; internal set; }

        /// <summary>
        /// Comission for submitting the order. This property value can change anytime after the order was submitted.
        /// </summary>
        public decimal Commission { get; internal set; }

        internal IBApi.Order IBOrderToPlace { get; set; }
        internal ManualResetEvent FinishLock { get; set; }
        internal ManualResetEvent CommissionLock { get; set; }
        internal ManualResetEvent AverageFillPriceLock { get; set; }




        /// <summary>
        /// This function waits until the order is in its last state.
        /// </summary>
        public void WailTillFinishes()
        {
            FinishLock.WaitOne();
        }

        /// <summary>
        /// This function waits until the order is in its last state and contain information about comissions and average fill price.
        /// </summary>
        public void WaitForExecutionDetails()
        {
            FinishLock.WaitOne();
            CommissionLock.WaitOne();
            AverageFillPriceLock.WaitOne();
        }
    }
}
