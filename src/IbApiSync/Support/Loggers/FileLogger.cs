using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Loggers
{
    public class FileLogger : Logger, ILogger
    {
        private readonly string DirectoryPath;
        private object Lock = new object();




        public FileLogger(string logDirectory)
        {
            DirectoryPath = logDirectory;

            // Create directory if doesn't exist
            Directory.CreateDirectory(DirectoryPath);
        }




        public new void Info(params string[] lines)
        {
            Flush(base.Info(lines));
        }

        public new void Warning(params string[] lines)
        {
            Flush(base.Warning(lines));
        }

        public new void Error(params string[] lines)
        {
            Flush(base.Error(lines));
        }
        public new void Error(Exception ex)
        {
            Flush(base.Error(ex));
        }
        public new void Error(string message, Exception ex)
        {
            Flush(base.Error(message, ex));
        }

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
