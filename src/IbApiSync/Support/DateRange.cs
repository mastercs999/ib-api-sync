using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support
{
    /// <summary>
    /// Just a wrapper for date range
    /// </summary>
    [Serializable]
    public class DateRange
    {
        /// <summary>
        /// Beginning of date range
        /// </summary>
        public DateTimeOffset From { get; set; }

        /// <summary>
        /// End of date range
        /// </summary>
        public DateTimeOffset To { get; set; }
    }
}
