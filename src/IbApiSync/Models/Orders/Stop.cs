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
    public class Stop : Order
    {
        private OrderAction _ActionToStop;
        public OrderAction ActionToStop
        {
            get => _ActionToStop;
            set => IBOrderToPlace.Action = (_ActionToStop = value).Reverse().Text();
        }

        public override OrderAction Action
        {
            get => ActionToStop.Reverse();
            set => ActionToStop = value.Reverse();
        }

        private decimal _StopPrice;
        public decimal StopPrice
        {
            get => _StopPrice;
            set => IBOrderToPlace.AuxPrice = _StopPrice = value;
        }

        private decimal _Quantity;
        public override decimal Quantity
        {
            get => _Quantity;
            set => IBOrderToPlace.TotalQuantity = _Quantity = value;
        }

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
