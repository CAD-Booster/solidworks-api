using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CADBooster.SolidDna
{
    public static class Logger
    {
        private static readonly Dictionary<Type, List<ILogger>> Loggers = new Dictionary<Type, List<ILogger>>();

        /// <summary>
        /// If true, any uncaught exceptions that get thrown will get caught, logged, then swallowed.
        /// 
        /// WARNING: If turning this on, be aware you may get null/default values
        /// being returned from function calls if they throw errors
        /// so be vigilant on null checking if so
        /// </summary>
        public static bool LogAndIgnoreUncaughtExceptions { get; set; } = false;

        /// <summary>
        /// Add a file logger for all SolidDna log messages.
        /// Is cleaned up when your add-in unloads.
        /// </summary>
        /// <typeparam name="TAddIn"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="configuration"></param>
        public static void AddFileLogger<TAddIn>(string filePath, FileLoggerConfiguration configuration) where TAddIn : SolidAddIn
        {
            if (configuration == null)
                configuration = new FileLoggerConfiguration();
            AddLogger<TAddIn>(new FileLogger("SolidDna", filePath, configuration));
        }

        /// <summary>
        /// Add a logger for all SolidDna log messages.
        /// Is cleaned up when your add-in unloads.
        /// </summary>
        /// <typeparam name="TAddIn"></typeparam>
        public static void AddLogger<TAddIn>(ILogger logger) where TAddIn : SolidAddIn
        {
            if (Loggers.ContainsKey(typeof(TAddIn)))
                Loggers[typeof(TAddIn)].Add(logger);
            else
                Loggers.Add(typeof(TAddIn), new List<ILogger> {logger});
        }

        /// <summary>
        /// Remove all loggers for a certain <see cref="SolidAddIn"/>.
        /// </summary>
        /// <param name="addIn"></param>
        internal static void RemoveLoggers(SolidAddIn addIn)
        {
            // Get the derived type of the add-in
            var type = addIn.GetType();

            // Remove the list of loggers for this add-in type
            Loggers.Remove(type);
        }

        /// <summary>
        /// Logs a critical message, including the source of the log
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="exception"></param>
        /// <param name="origin">The callers member/function name</param>
        /// <param name="filePath">The source code file path</param>
        /// <param name="lineNumber">The line number in the code file of the caller</param>
        /// <param name="args">The additional arguments</param>
        /// <param name="eventId"></param>
        public static void LogCriticalSource(
            string message,
            EventId eventId = new EventId(),
            Exception exception = null,
            [CallerMemberName] string origin = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            params object[] args) =>
            LogToAllLoggers(LogLevel.Critical, message, eventId, exception, origin, filePath, lineNumber, args);

        /// <summary>
        /// Logs a verbose trace message, including the source of the log
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="exception"></param>
        /// <param name="origin">The callers member/function name</param>
        /// <param name="filePath">The source code file path</param>
        /// <param name="lineNumber">The line number in the code file of the caller</param>
        /// <param name="args">The additional arguments</param>
        /// <param name="eventId"></param>
        public static void LogTraceSource(
            string message,
            EventId eventId = new EventId(),
            Exception exception = null,
            [CallerMemberName] string origin = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            params object[] args)=>
            LogToAllLoggers(LogLevel.Trace, message, eventId, exception, origin, filePath, lineNumber, args);

        /// <summary>
        /// Logs a debug message, including the source of the log
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="exception"></param>
        /// <param name="origin">The callers member/function name</param>
        /// <param name="filePath">The source code file path</param>
        /// <param name="lineNumber">The line number in the code file of the caller</param>
        /// <param name="args">The additional arguments</param>
        /// <param name="eventId"></param>
        public static void LogDebugSource(
            string message,
            EventId eventId = new EventId(),
            Exception exception = null,
            [CallerMemberName] string origin = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            params object[] args)=>
            LogToAllLoggers(LogLevel.Debug, message, eventId, exception, origin, filePath, lineNumber, args);

        /// <summary>
        /// Logs an error message, including the source of the log
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="eventId"></param>
        /// <param name="exception"></param>
        /// <param name="origin">The callers member/function name</param>
        /// <param name="filePath">The source code file path</param>
        /// <param name="lineNumber">The line number in the code file of the caller</param>
        /// <param name="args">The additional arguments</param>
        public static void LogErrorSource(
            string message,
            EventId eventId = new EventId(),
            Exception exception = null,
            [CallerMemberName] string origin = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            params object[] args)=>
            LogToAllLoggers(LogLevel.Error, message, eventId, exception, origin, filePath, lineNumber, args);

        /// <summary>
        /// Logs an informative message, including the source of the log
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="eventId"></param>
        /// <param name="exception"></param>
        /// <param name="origin">The callers member/function name</param>
        /// <param name="filePath">The source code file path</param>
        /// <param name="lineNumber">The line number in the code file of the caller</param>
        /// <param name="args">The additional arguments</param>
        public static void LogInformationSource(
            string message,
            EventId eventId = new EventId(),
            Exception exception = null,
            [CallerMemberName] string origin = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            params object[] args)=>
            LogToAllLoggers(LogLevel.Information, message, eventId, exception, origin, filePath, lineNumber, args);

        /// <summary>
        /// Logs a warning message, including the source of the log
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="eventId"></param>
        /// <param name="exception"></param>
        /// <param name="origin">The callers member/function name</param>
        /// <param name="filePath">The source code file path</param>
        /// <param name="lineNumber">The line number in the code file of the caller</param>
        /// <param name="args">The additional arguments</param>
        public static void LogWarningSource(
            string message,
            EventId eventId = new EventId(),
            Exception exception = null,
            [CallerMemberName] string origin = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            params object[] args)=>
            LogToAllLoggers(LogLevel.Warning, message, eventId, exception, origin, filePath, lineNumber, args);

        /// <summary>
        /// Write a message to all attached loggers.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        /// <param name="exception"></param>
        /// <param name="origin"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        /// <param name="args"></param>
        private static void LogToAllLoggers(LogLevel logLevel, string message, EventId eventId, Exception exception,
            string origin, string filePath, int lineNumber, object[] args)
        {
            // Write to debugger (only in Debug mode) and the console for easier debugging
            var completeMessage = $"{logLevel}: {message}";
            Debug.WriteLine(completeMessage);
            Console.WriteLine(completeMessage);

            // Write to all other loggers
            foreach (var loggers in Loggers.Values)
            {
                foreach (var logger in loggers)
                {
                    logger.Log(logLevel, eventId, args.Prepend(origin, filePath, lineNumber, message), exception, LoggerSourceFormatter.Format);
                }
            }
        }
    }
}
