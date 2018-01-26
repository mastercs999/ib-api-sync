using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support
{
    public enum ProductType
    {
        [Description("STK")]
        Stock,
        [Description("CFD")]
        CFD
    }

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

    public enum OrderAction
    {
        [Description("BUY")]
        Buy,
        [Description("SELL")]
        Sell
    }

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
