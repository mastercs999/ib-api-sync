using IbApiSync.Support;
using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models
{
    public class Product
    {
        public int Id { get; internal set; }
        public string Symbol { get; internal set; }
        public string Exchange { get; internal set; }
        public string Currency { get; internal set; }
        public List<DateRange> TradingHours { get; internal set; }

        internal Contract IBContract { get; set; }
        internal ContractDetails IBContractDetails { get; set; }
    }
}
