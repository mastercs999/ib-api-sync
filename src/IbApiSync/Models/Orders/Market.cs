using IbApiSync.Support;
using Common;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models.Orders
{
    public class Market : Order
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
