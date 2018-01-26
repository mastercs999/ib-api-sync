using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Exceptions
{
    /// <summary>
    /// Exception indicating any problem with connection.
    /// </summary>
    public class ConnectionException : Exception
    {
        /// <summary>
        /// Creates instance of this class
        /// </summary>
        public ConnectionException() : base()
        {
        }

        /// <summary>
        /// Creates instance of this class with specified message.
        /// </summary>
        /// <param name="message">Message of this exception</param>
        public ConnectionException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="message">Message of this exception.</param>
        /// <param name="inner">Inner exception describing exception in more details.</param>
        public ConnectionException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
