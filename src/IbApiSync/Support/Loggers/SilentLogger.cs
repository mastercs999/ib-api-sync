using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Loggers
{
    public class SilentLogger : Logger, ILogger
    {
        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="lines">Line or mutliple lines for log.</param>
        public new void Info(params string[] lines)
        {
            ;
        }




        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="lines">Line or mutliple lines for log.</param>
        public new void Warning(params string[] lines)
        {
            ;
        }




        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="lines">Line or mutliple lines for log.</param>
        public new void Error(params string[] lines)
        {
            ;
        }

        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="ex">Exception for log.</param>
        public new void Error(Exception ex)
        {
            ;
        }

        /// <summary>
        /// This method does nothing.
        /// </summary>
        /// <param name="message">Text of the error.</param>
        /// <param name="ex">Exception for log.</param>
        public new void Error(string message, Exception ex)
        {
            ;
        }




        /// <summary>
        /// This method does nothig.
        /// </summary>
        /// <param name="methodName">Name of the invoked method.</param>
        /// <param name="arguments">Arguments of invoked method. Usually alternating sequence of name of the argument and its value.</param>
        public new void WriteMethod(string methodName, params object[] arguments)
        {
            ;
        }
    }
}
