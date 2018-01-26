using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Loggers
{
    /// <summary>
    /// This logger writes all its messages into console (surprisingly).
    /// </summary>
    public class ConsoleLogger : Logger, ILogger
    {
        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="lines">Line or multiple lines to be written into log.</param>
        public new void Info(params string[] lines)
        {
            Console.Write(base.Info(lines));
        }




        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="lines">Line or multiple lines to be written into log.</param>
        public new void Warning(params string[] lines)
        {
            Console.Write(base.Warning(lines));
        }




        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="lines">Line or multiple lines to be written into log.</param>
        public new void Error(params string[] lines)
        {
            Console.Write(base.Error(lines));
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="ex">Exception to log.</param>
        public new void Error(Exception ex)
        {
            Console.Write(base.Error(ex));
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">Text of the logged error.</param>
        /// <param name="ex">Exception to log.</param>
        public new void Error(string message, Exception ex)
        {
            Console.Write(base.Error(message, ex));
        }




        /// <summary>
        /// Writes an info log message about invoked method.
        /// </summary>
        /// <param name="methodName">Name of the invoked method.</param>
        /// <param name="arguments">Arguments of invoked method. Usually alternating sequence of name of the argument and its value.</param>
        public new void WriteMethod(string methodName, params object[] arguments)
        {
            Console.Write(base.WriteMethod(methodName, arguments));
        }
    }
}
