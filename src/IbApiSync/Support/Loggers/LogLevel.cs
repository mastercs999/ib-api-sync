using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Loggers
{
    /// <summary>
    /// Enum indicating severity of log message.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Simple informative log record.
        /// </summary>
        Info,

        /// <summary>
        /// Log level with warning.
        /// </summary>
        Warning,

        /// <summary>
        /// Log level for errors.
        /// </summary>
        Error
    }
}
