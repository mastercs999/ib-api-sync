using IbApiSync.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IbApiSync.Models
{
    public abstract class Order
    {
        public virtual decimal Quantity { get; set; }
        public virtual OrderAction Action { get; set; }

        public OrderStatus Status { get; internal set; }
        public Product Product { get; internal set; }
        public decimal AverageFillPrice { get; internal set; }
        public decimal Commission { get; internal set; }

        internal IBApi.Order IBOrderToPlace { get; set; }
        internal ManualResetEvent FinishLock { get; set; }
        internal ManualResetEvent CommissionLock { get; set; }
        internal ManualResetEvent AverageFillPriceLock { get; set; }

        public void WailTillFinishes()
        {
            FinishLock.WaitOne();
        }

        public void WaitForExecutionDetails()
        {
            FinishLock.WaitOne();
            CommissionLock.WaitOne();
            AverageFillPriceLock.WaitOne();
        }
    }
}
