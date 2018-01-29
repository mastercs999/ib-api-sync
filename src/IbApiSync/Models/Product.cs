using IbApiSync.Support;
using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models
{
    /// <summary>
    /// This class represents a product which can be obtained from IB API.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Id of the product.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Product's symbol.
        /// </summary>
        public string Symbol { get; internal set; }

        /// <summary>
        /// Exchange where the product is traded. Universal exchange is SMART exchange.
        /// </summary>
        public string Exchange { get; internal set; }

        /// <summary>
        /// Currency in which the product is denominated.
        /// </summary>
        public string Currency { get; internal set; }

        /// <summary>
        /// Next N daytime intervals when the product is traded.
        /// </summary>
        public List<DateRange> TradingHours { get; internal set; }




        internal Contract IBContract { get; set; }
        internal ContractDetails IBContractDetails { get; set; }
    }
}
