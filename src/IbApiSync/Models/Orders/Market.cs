using IbApiSync.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models.Orders
{
    /// <summary>
    /// This class represents the most simple order - Market order.
    /// </summary>
    public class Market : Order
    {
        /// <summary>
        /// Direction of this order.
        /// </summary>
        public override OrderAction Action
        {
            get => _Action;
            set => IBOrderToPlace.Action = (_Action = value).Text();
        }
        private OrderAction _Action;

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
        /// Creates the market order with specified options.
        /// </summary>
        /// <param name="product">Target product of this order.</param>
        /// <param name="action">Direction of the order.</param>
        /// <param name="quantity">Number of shares or just 'units' for this order.</param>
        public Market(Product product, OrderAction action, decimal quantity)
        {
            Product = product;
            IBOrderToPlace = new IBApi.Order()
            {
                OrderType = OrderType.Market.Text(),
            };

            Action = action;
            Quantity = quantity;
        }
    }
}
