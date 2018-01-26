using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Loggers
{
    public class SilentLogger : Logger, ILogger
    {
        public new void Info(params string[] lines)
        {
            ;
        }

        public new void Warning(params string[] lines)
        {
            ;
        }

        public new void Error(params string[] lines)
        {
            ;
        }
        public new void Error(Exception ex)
        {
            ;
        }
        public new void Error(string message, Exception ex)
        {
            ;
        }

        public new void WriteMethod(string methodName, params object[] arguments)
        {
            ;
        }
    }
}
