using IbApiSync.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models.Orders
{
    /// <summary>
    /// Represents stop order mostly used as a stop loss.
    /// </summary>
    public class Stop : Order
    {
        /// <summary>
        /// Direction of the position to be stopped by this order.
        /// </summary>
        public OrderAction ActionToStop
        {
            get => _ActionToStop;
            set => IBOrderToPlace.Action = (_ActionToStop = value).Reverse().Text();
        }
        private OrderAction _ActionToStop;

        /// <summary>
        /// Action to be taken on trigger.
        /// </summary>
        public override OrderAction Action
        {
            get => ActionToStop.Reverse();
            set => ActionToStop = value.Reverse();
        }

        /// <summary>
        /// Triggering price for this order.
        /// </summary>
        public decimal StopPrice
        {
            get => _StopPrice;
            set => IBOrderToPlace.AuxPrice = _StopPrice = value;
        }
        private decimal _StopPrice;

        /// <summary>
        /// Number of shares/units of this order.
        /// </summary>
        public override decimal Quantity
        {
            get => _Quantity;
            set => IBOrderToPlace.TotalQuantity = _Quantity = value;
        }
        private decimal _Quantity;




        /// <summary>
        /// Creates this order with specified parameters.
        /// </summary>
        /// <param name="product">Target product of this order.</param>
        /// <param name="actionToStop">Direction of the position to be stopped by this order.</param>
        /// <param name="stopPrice">Triggering price.</param>
        /// <param name="quantity">Number of shares or just 'units' for this order.</param>
        public Stop(Product product, OrderAction actionToStop, decimal stopPrice, decimal quantity)
        {
            Product = product;
            IBOrderToPlace = new IBApi.Order()
            {
                OrderType = OrderType.Stop.Text(),
            };

            ActionToStop = actionToStop;
            StopPrice = stopPrice;
            Quantity = quantity;
        }
    }
}
