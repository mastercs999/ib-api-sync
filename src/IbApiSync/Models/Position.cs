using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models
{
    public class Position
    {
        public Product Product { get; internal set; }
        public decimal Size { get; internal set; }
        public decimal UnitCost { get; internal set; }

        internal string Account { get; set; }
    }
}
