using System;
using System.IO;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Formats a message when the callers source information is provided first in the arguments
    /// </summary>
    public static class LoggerSourceFormatter
    {
        /// <summary>
        /// Formats the message including the source information pulled out of the state
        /// </summary>
        /// <param name="state">The state information about the log</param>
        /// <param name="exception">The exception</param>
        /// <returns></returns>
        public static string Format(object[] state, Exception exception)
        {
            // Get the values from the state
            var origin = (string)state[0];
            var filePath = (string)state[1];
            var lineNumber = (int)state[2];
            var message = (string)state[3];

            // Get any exception message
            var exceptionMessage = exception?.ToString();

            // If we have an exception ...
            if (exception != null)
                // New line between message and exception
                exceptionMessage = Environment.NewLine + exception;

            // Format the message string
            return $"{message} [{Path.GetFileName(filePath)} > {origin}() > Line {lineNumber}]{exceptionMessage}";
        }
    }
}
