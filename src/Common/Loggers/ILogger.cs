using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Loggers
{
    public interface ILogger
    {
        string LastMessage { get; }

        void Info(params string[] lines);
        void Warning(params string[] lines);
        void Error(params string[] lines);
        void Error(Exception ex);
        void Error(string message, Exception ex);
        void WriteMethod(string methodName, params object[] arguments);
    }
}
