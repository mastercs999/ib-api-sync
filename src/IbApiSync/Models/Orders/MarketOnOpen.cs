using IbApiSync.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models.Orders
{
    public class MarketOnOpen : Order
    {
        private OrderAction _Action;
        public override OrderAction Action
        {
            get => _Action;
            set => IBOrderToPlace.Action = (_Action = value).Text();
        }

        private decimal _Quantity;
        public override decimal Quantity
        {
            get => _Quantity;
            set => IBOrderToPlace.TotalQuantity = _Quantity = value;
        }

        public MarketOnOpen(Product product, OrderAction action, decimal quantity)
        {
            Product = product;
            IBOrderToPlace = new IBApi.Order()
            {
                OrderType = OrderType.MarketOnOpen.Text(),
                Tif = "OPG"
            };

            Action = action;
            Quantity = quantity;
        }
    }
}
