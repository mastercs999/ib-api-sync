using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Models
{
    /// <summary>
    /// Position which is holded by the account.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Product for which the account hold an open position.
        /// </summary>
        public Product Product { get; internal set; }

        /// <summary>
        /// Size of the position. For example number of shares.
        /// </summary>
        public decimal Size { get; internal set; }

        /// <summary>
        /// Current value of one unit. Apparently position values is <see cref="UnitCost"/> * <see cref="Size"/>.
        /// </summary>
        public decimal UnitCost { get; internal set; }
        



        internal string Account { get; set; }
    }
}
