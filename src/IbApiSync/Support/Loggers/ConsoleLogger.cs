using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Loggers
{
    public class ConsoleLogger : Logger, ILogger
    {
        public new void Info(params string[] lines)
        {
            Console.Write(base.Info(lines));
        }

        public new void Warning(params string[] lines)
        {
            Console.Write(base.Warning(lines));
        }

        public new void Error(params string[] lines)
        {
            Console.Write(base.Error(lines));
        }
        public new void Error(Exception ex)
        {
            Console.Write(base.Error(ex));
        }
        public new void Error(string message, Exception ex)
        {
            Console.Write(base.Error(message, ex));
        }

        public new void WriteMethod(string methodName, params object[] arguments)
        {
            Console.Write(base.WriteMethod(methodName, arguments));
        }
    }
}
