using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Loggers
{
    /// <summary>
    /// This logger writes all its messages into files. For single day there is a log file with day stamp in its name. 
    /// </summary>
    /// <remarks>
    /// Because the file can be sometimes heavily access it is advised to
    /// remove this file from antivirus scope.
    /// </remarks>
    public class FileLogger : Logger, ILogger
    {
        private readonly string DirectoryPath;
        private object Lock = new object();




        /// <summary>
        /// Creates instance of this class and makes sure that target directory exists.
        /// </summary>
        /// <param name="logDirectory">Directory for log files.</param>
        public FileLogger(string logDirectory)
        {
            DirectoryPath = logDirectory;

            // Create directory if doesn't exist
            Directory.CreateDirectory(DirectoryPath);
        }




        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="lines">Line or multiple lines to be written into log.</param>
        public new void Info(params string[] lines)
        {
            Flush(base.Info(lines));
        }




        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="lines">Line or multiple lines to be written into log.</param>
        public new void Warning(params string[] lines)
        {
            Flush(base.Warning(lines));
        }




        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="lines">Line or multiple lines to be written into log.</param>
        public new void Error(params string[] lines)
        {
            Flush(base.Error(lines));
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="ex">Exception to log.</param>
        public new void Error(Exception ex)
        {
            Flush(base.Error(ex));
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">Text of the logged error.</param>
        /// <param name="ex">Exception to log.</param>
        public new void Error(string message, Exception ex)
        {
            Flush(base.Error(message, ex));
        }




        /// <summary>
        /// Writes an info log message about invoked method.
        /// </summary>
        /// <param name="methodName">Name of the invoked method.</param>
        /// <param name="arguments">Arguments of invoked method. Usually alternating sequence of name of the argument and its value.</param>
        public new void WriteMethod(string methodName, params object[] arguments)
        {
            Flush(base.WriteMethod(methodName, arguments));
        }




        private void Flush(string content)
        {
            // Write to the file
            lock (Lock)
                File.AppendAllText(Path.Combine(DirectoryPath, DateTimeOffset.UtcNow.ToString("yyyy_MM_dd") + ".log"), content);
        }
    }
}
