using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Exceptions
{
    /// <summary>
    /// Exception sent by IB Gateway.
    /// </summary>
    public class IBException : Exception
    {
        /// <summary>
        /// Id of an error.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Error code of an error.
        /// </summary>
        public int ErrorCode { get; private set; }




        /// <summary>
        /// Creates instance of this class
        /// </summary>
        public IBException() : base()
        {
        }

        /// <summary>
        /// Creates instance of this class with specified message.
        /// </summary>
        /// <param name="message">Message of this exception</param>
        public IBException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="message">Message of this exception.</param>
        /// <param name="inner">Inner exception describing exception in more details.</param>
        public IBException(string message, Exception inner)
        : base(message, inner)
        {
        }

        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="message">Message of this exception.</param>
        /// <param name="id">Id of IB error.</param>
        /// <param name="errorCode">Error code of IB error.</param>
        public IBException(string message, int id, int errorCode) : base(message)
        {
            Id = id;
            ErrorCode = errorCode;
        }
    }
}
