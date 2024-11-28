using System;

namespace CADBooster.SolidDna
{
    //
    // Summary:
    //     Represents a type used to perform logging. Copied from Microsoft.Extensions.Logging
    //
    // Remarks:
    //     Aggregates most logging patterns to a single method.
    public interface ILogger
    {
        //
        // Summary:
        //     Begins a logical operation scope.
        //
        // Parameters:
        //   state:
        //     The identifier for the scope.
        //
        // Returns:
        //     An IDisposable that ends the logical operation scope on dispose.
        IDisposable BeginScope<TState>(TState state);
        //
        // Summary:
        //     Checks if the given logLevel is enabled.
        //
        // Parameters:
        //   logLevel:
        //     level to be checked.
        //
        // Returns:
        //     true if enabled.
        bool IsEnabled(LogLevel logLevel);
        //
        // Summary:
        //     Writes a log entry.
        //
        // Parameters:
        //   logLevel:
        //     Entry will be written on this level.
        //
        //   eventId:
        //     ID of the event.
        //
        //   state:
        //     The entry to be written. Can be also an object.
        //
        //   exception:
        //     The exception related to this entry.
        //
        //   formatter:
        //     Function to create a string message of the state and exception.
        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
    }
}
