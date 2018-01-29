using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support
{
    /// <summary>
    /// Product types of products on IB.
    /// </summary>
    public enum ProductType
    {
        [Description("STK")]
        Stock,
        [Description("CFD")]
        CFD
    }

    /// <summary>
    /// Types of available/implemented orders.
    /// </summary>
    public enum OrderType
    {
        [Description("MKT")]
        Market,
        [Description("MOC")]
        MarketOnClose,
        [Description("MKT")]
        MarketOnOpen,
        [Description("STP")]
        Stop
    }

    /// <summary>
    /// Direction of an order - buy or sell.
    /// </summary>
    public enum OrderAction
    {
        [Description("BUY")]
        Buy,
        [Description("SELL")]
        Sell
    }

    /// <summary>
    /// All possible states of an order.
    /// </summary>
    public enum OrderStatus
    {
        NotPlaced,
        PendingSubmit,
        PendingCancel,
        PreSubmitted,
        Submitted,
        Cancelled,
        Filled,
        Inactive
    }
}
