using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Loggers
{
    /// <summary>
    /// Interface for logging
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Provides simple information about last logged message.
        /// </summary>
        string LastMessage { get; }




        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="lines">Line or multiple lines to be written into log.</param>
        void Info(params string[] lines);




        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="lines">Line or multiple lines to be written into log.</param>
        void Warning(params string[] lines);




        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="lines">Line or multiple lines to be written into log.</param>
        void Error(params string[] lines);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="ex">Exception to log.</param>
        void Error(Exception ex);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">Text of the logged error.</param>
        /// <param name="ex">Exception to log.</param>
        void Error(string message, Exception ex);




        /// <summary>
        /// Writes an info log message about invoked method.
        /// </summary>
        /// <param name="methodName">Name of the invoked method.</param>
        /// <param name="arguments">Arguments of invoked method. Usually alternating sequence of name of the argument and its value.</param>
        void WriteMethod(string methodName, params object[] arguments);
    }
}
