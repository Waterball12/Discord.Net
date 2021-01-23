using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Logging
{
    internal class DefaultLogger : ILogger
    {
        private static readonly object _lock = new object();

        private LogLevel MinimumLevel { get; }
        private string TimestampFormat { get; }

        internal DefaultLogger(LogLevel minLevel = LogLevel.Information, string timestampFormat = "yyyy-MM-dd HH:mm:ss zzz")
        {
            this.MinimumLevel = minLevel;
            this.TimestampFormat = timestampFormat;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
                return;

            lock (_lock)
            {
                var ename = eventId.Name;
                ename = ename?.Length > 12 ? ename?.Substring(0, 12) : ename;
                Console.Write($"[{DateTimeOffset.Now.ToString(this.TimestampFormat)}] [{eventId.Id,-4}/{ename,-12}] ");
                
                Console.Write(logLevel switch 
                {
                    LogLevel.Trace =>       "[Trace] ",
                    LogLevel.Debug =>       "[Debug] ",
                    LogLevel.Information => "[Info ] ",
                    LogLevel.Warning =>     "[Warn ] ",
                    LogLevel.Error =>       "[Error] ",
                    LogLevel.Critical =>    "[Crit ]",
                    LogLevel.None =>        "[None ] ",
                    _ =>                    "[?????] "
                });

                //The foreground color is off.
                if (logLevel == LogLevel.Critical)
                    Console.Write(" ");

                var message = formatter(state, exception);
                Console.WriteLine(message);
                if (exception != null)
                    Console.WriteLine(exception);
            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= this.MinimumLevel;

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
