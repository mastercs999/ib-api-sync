using IbApiSync.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models.Orders
{
    /// <summary>
    /// Represents market order which should be executed just on current trading day close.
    /// </summary>
    public class MarketOnClose : Order
    {
        /// <summary>
        /// Direction of the order.
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
        /// Creates this order with specified parameters.
        /// </summary>
        /// <param name="product">Target product of this order.</param>
        /// <param name="action">Direction of the order.</param>
        /// <param name="quantity">Number of shares or just 'units' for this order.</param>
        public MarketOnClose(Product product, OrderAction action, decimal quantity)
        {
            Product = product;
            IBOrderToPlace = new IBApi.Order()
            {
                OrderType = OrderType.MarketOnClose.Text(),
            };

            Action = action;
            Quantity = quantity;
        }
    }
}
