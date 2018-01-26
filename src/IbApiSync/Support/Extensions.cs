using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support
{
    public static class Extensions
    {
        public static OrderAction Reverse(this OrderAction orderAction)
        {
            return orderAction == OrderAction.Buy ? OrderAction.Sell : OrderAction.Buy;
        }

        public static bool IsLastStatus(this OrderStatus orderStatus)
        {
            return orderStatus == OrderStatus.Filled || orderStatus == OrderStatus.Cancelled || orderStatus == OrderStatus.Inactive;
        }
    }
}
