using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Exceptions
{
    public class ConnectionException : Exception
    {
        public ConnectionException() : base()
        {
        }

        public ConnectionException(string message)
        : base(message)
        {
        }

        public ConnectionException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
