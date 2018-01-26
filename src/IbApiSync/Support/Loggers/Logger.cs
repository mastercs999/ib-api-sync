using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support.Loggers
{
    public abstract class Logger
    {
        public string LastMessage { get; private set; }

        protected string Timestamp => DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff");

        private string ThisNamespace = typeof(Logger).Namespace;



        protected string Info(params string[] lines)
        {
            LastMessage = lines?.FirstOrDefault();

            return CreateLogMessage(lines, LogLevel.Info);
        }

        protected string Warning(params string[] lines)
        {
            LastMessage = lines?.FirstOrDefault();

            return CreateLogMessage(lines, LogLevel.Warning);
        }

        protected string Error(params string[] lines)
        {
            LastMessage = lines?.FirstOrDefault();

            return CreateLogMessage(lines, LogLevel.Error);
        }
        protected string Error(Exception ex)
        {
            LastMessage = ex?.Message;

            List<string> lines = FormatException(ex);

            return CreateLogMessage(lines.ToArray(), LogLevel.Error);
        }
        protected string Error(string message, Exception ex)
        {
            LastMessage = message;
            
            List<string> lines = FormatException(ex);
            lines.Insert(0, message);

            return CreateLogMessage(lines.ToArray(), LogLevel.Error);
        }

        protected string WriteMethod(string methodName, params object[] arguments)
        {
            LastMessage = "<METHOD> " + methodName;

            // Log header
            List<string> lines = new List<string>()
            {
                methodName.Pad() + "<METHOD>"
            };

            // Log arguments
            for (int i = 0; i < arguments.Length; i += 2)
            {
                lines.Add("-------------");
                lines.Add((arguments[i] as string).Pad() + "<ARG_NAME>");
                lines.Add(arguments[i + 1].Dump());
            }

            return CreateLogMessage(lines.ToArray(), LogLevel.Info);
        }




        private List<string> FormatException(Exception ex)
        {
            List<string> lines = new List<string>();

            while (ex != null)
            {
                lines.AddRange(new string[]
                {
                    "<EXCEPTION>",
                    ex.Message,
                    "\n",
                    ex.StackTrace,
                    "\n",
                    ex.ToString(),
                    "\n",
                    "<INNER EXCEPTION>".Pad() + (ex.InnerException == null ? "<NULL>" : "<FOLLOWS>")
                });

                ex = ex.InnerException;
            }

            return lines;
        }
        private string CreateLogMessage(string[] lines, LogLevel logLevel)
        {
            // Make sure there are no new lines between
            lines = String.Join("\n", lines).Split(new string[] { "\n" }, StringSplitOptions.None);

            // Get timestamp
            string timestamp = Timestamp;

            // Get caller information
            List<Caller> callers = FindCallers();

            // Append header
            string header = timestamp + " " + logLevel.Text() + " | " + String.Join(" <= ", callers.Select(x => $"{x.MethodName.Dump()} ({x.ClassName.Dump()})"));
            for (int i = 0; i < lines.Length; ++i)
                lines[i] = new string(' ', timestamp.Length + 1) + lines[i];

            // Return as a string
            return header + "\n" + String.Join("\n", lines) + "\n";
        }

        private List<Caller> FindCallers()
        {
            StackTrace stack = new StackTrace();
            List<Caller> callers = new List<Caller>(stack.FrameCount);

            for (int i = 0; i < stack.FrameCount; ++i)
            {
                // Find calling type
                MethodBase callingMethod = stack.GetFrame(i)?.GetMethod();
                Type callingType = callingMethod?.DeclaringType;
                if (callingMethod == null || callingType == null || callingType.Namespace == ThisNamespace)
                    continue;

                callers.Add(new Caller()
                {
                    ClassName = callingType.GetRealTypeName(),
                    MethodName = callingMethod.Name
                });
            }

            return callers;
        }

        private class Caller
        {
            public string ClassName { get; set; }
            public string MethodName { get; set; }
        }
    }
}
