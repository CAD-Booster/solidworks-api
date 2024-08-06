using System;
using System.Collections.Concurrent;
using System.IO;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// A logger that writes the logs to file
    /// </summary>
    public class FileLogger : ILogger
    {
        #region Static Properties

        /// <summary>
        /// A list of file locks based on path
        /// </summary>
        protected static ConcurrentDictionary<string, object> FileLocks = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// The lock to lock the list of locks
        /// </summary>
        protected static object FileLockLock = new object();

        #endregion

        #region Protected Members

        /// <summary>
        /// The category for this logger
        /// </summary>
        protected readonly string mCategoryName;

        /// <summary>
        /// The file path to write to
        /// </summary>
        protected readonly string mFilePath;

        /// <summary>
        /// The directory the file is in
        /// </summary>
        protected readonly string mDirectory;

        /// <summary>
        /// The configuration to use
        /// </summary>
        protected FileLoggerConfiguration mConfiguration;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="categoryName">The category for this logger</param>
        /// <param name="filePath">The file path to write to</param>
        /// <param name="configuration">The configuration to use</param>
        public FileLogger(string categoryName, string filePath, FileLoggerConfiguration configuration)
        {
            // Get absolute path
            filePath = Path.GetFullPath(filePath);

            // Set members
            mCategoryName = categoryName;
            mFilePath = filePath;
            mDirectory = Path.GetDirectoryName(filePath);
            mConfiguration = configuration;
        }

        #endregion

        /// <summary>
        /// File loggers are not scoped so this is always null
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Enabled if the log level is the same or greater than the configuration
        /// </summary>
        /// <param name="logLevel">The log level to check against</param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            // Enabled if the log level is greater or equal to what we want to log
            return logLevel >= mConfiguration.LogLevel;
        }

        /// <summary>
        /// Logs the message to file
        /// </summary>
        /// <typeparam name="TState">The type of the details of the message</typeparam>
        /// <param name="logLevel">The log level</param>
        /// <param name="eventId">The ID</param>
        /// <param name="state">The details of the message</param>
        /// <param name="exception">Any exception to log</param>
        /// <param name="formatter">The formatter for converting the state and exception to a message string</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // If we should not log...
            if (!IsEnabled(logLevel))
                // Return
                return;

            // Get current time
            var currentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");

            // Prepend log level
            var logLevelString = mConfiguration.OutputLogLevel ? $"{logLevel.ToString().ToUpper()}: " : "";

            // Prepend the time to the log if desired
            var timeLogString = mConfiguration.LogTime ? $"[{currentTime}] " : "";

            // Get the formatted message string
            var message = formatter(state, exception);

            // Write the message
            var output = $"{logLevelString}{timeLogString}{message}{Environment.NewLine}";

            // Normalize path
            // TODO: Make use of configuration base path
            var normalizedPath = mFilePath.ToUpper();

            var fileLock = default(object);

            // Double safety even though the FileLocks should be thread safe
            lock (FileLockLock)
            {
                // Get the file lock based on the absolute path
                fileLock = FileLocks.GetOrAdd(normalizedPath, path => new object());
            }

            // Lock the file
            lock (fileLock)
            {
                // Ensure folder
                if (!Directory.Exists(mDirectory))
                    Directory.CreateDirectory(mDirectory);

                // Open the file
                using (var fileStream = new StreamWriter(File.Open(mFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)))
                {
                    // Go to end
                    fileStream.BaseStream.Seek(0, SeekOrigin.End);

                    // NOTE: Ignore logToTop in configuration as not efficient for files on OS

                    // Write the message to the file
                    fileStream.Write(output);
                }
            }
        }
    }
}
