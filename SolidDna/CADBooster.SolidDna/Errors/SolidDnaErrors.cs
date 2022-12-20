using System;
using System.Threading.Tasks;

namespace CADBooster.SolidDna
{
    /// <summary>
    /// Provides details and lookups for SolidDNA errors and codes
    /// </summary>
    public static class SolidDnaErrors
    {
        #region Public Methods

        /// <summary>
        /// Creates a <see cref="SolidDnaError"/> from the given details
        /// </summary>
        /// <param name="errorTypeCode">The error type code of this error</param>
        /// <param name="errorCode">The specific error code of this error</param>
        /// <param name="errorDetails">Specific details about this exact error. Will get localized first.</param>
        /// <param name="innerException">If an inner exception is supplied, its message is appended to the errorDetails</param>
        /// <returns></returns>
        public static SolidDnaError CreateError(SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDetails = null, Exception innerException = null)
        {
            var translatedErrorCode = Localization.GetString(errorCode.ToString());
            var errorMessage = errorDetails.IsNullOrWhiteSpace() ? translatedErrorCode : $"{translatedErrorCode}. {errorDetails}";

            // Create the error
            var error = new SolidDnaError
            {
                ErrorCodeValue = errorCode,
                ErrorMessage = errorMessage,
                ErrorTypeCodeValue = errorTypeCode,
            };

            // Set inner details
            if (innerException != null)
                error.ErrorDetails = $"{error.ErrorDetails}. Inner Exception ({innerException.GetType()}: {innerException.GetErrorMessage()}";

            return error;
        }

        /// <summary>
        /// Runs an action and catches any exceptions thrown
        /// wrapping and rethrowing them as a <see cref="SolidDnaException"/>
        /// </summary>
        /// <param name="action">The action to run</param>
        /// <param name="errorTypeCode">The <see cref="SolidDnaErrorTypeCode"/> to wrap the exception in</param>
        /// <param name="errorCode">The <see cref="SolidDnaErrorCode"/> to wrap the exception in</param>
        /// <param name="errorDescription">The description of the error if thrown. Gets localized before being thrown</param>
        public static void Wrap(Action action, SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDescription = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                // Create the SolidDNA exception
                var error = new SolidDnaException(CreateError(errorTypeCode, errorCode, errorDescription), ex);

                // If it should just be logged and ignored, log it
                if (Logger.LogAndIgnoreUncaughtExceptions)
                {
                    // Log the error
                    Logger.LogCriticalSource($"SolidDNA Exception created. {error.SolidDnaError}");
                    if (error.InnerException != null)
                        Logger.LogCriticalSource($"Inner Exception: { error.InnerException.GetErrorMessage()}");
                }
                // Otherwise, throw 
                else
                    throw error;
            }
        }

        /// <summary>
        /// Runs a function and catches any exceptions thrown, 
        /// wrapping and rethrowing them as a <see cref="SolidDnaException"/>
        /// </summary>
        /// <param name="func">The function to run</param>
        /// <param name="errorTypeCode">The <see cref="SolidDnaErrorTypeCode"/> to wrap the exception in</param>
        /// <param name="errorCode">The <see cref="SolidDnaErrorCode"/> to wrap the exception in</param>
        /// <param name="errorDescription">The description of the error if thrown. Gets localized before being thrown</param>
        public static T Wrap<T>(Func<T> func, SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDescription = null)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                // Create the SolidDNA exception
                var error = new SolidDnaException(CreateError(errorTypeCode, errorCode, errorDescription), ex);

                // If it should just be logged and ignored, log it
                if (Logger.LogAndIgnoreUncaughtExceptions)
                {
                    // Log the error
                    Logger.LogCriticalSource($"SolidDNA Exception created. {error.SolidDnaError}");
                    if (error.InnerException != null)
                        Logger.LogCriticalSource($"Inner Exception: { error.InnerException.GetErrorMessage()}");

                    return default;
                }
                // Otherwise, throw 
                throw error;
            }
        }

        /// <summary>
        /// Runs a task and catches any exceptions thrown, 
        /// wrapping and rethrowing them as a <see cref="SolidDnaException"/>
        /// </summary>
        /// <param name="task">The task to run</param>
        /// <param name="errorTypeCode">The <see cref="SolidDnaErrorTypeCode"/> to wrap the exception in</param>
        /// <param name="errorCode">The <see cref="SolidDnaErrorCode"/> to wrap the exception in</param>
        /// <param name="errorDescription">The description of the error if thrown. Gets localized before being thrown</param>
        public static async Task WrapAwaitAsync(Func<Task> task, SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDescription = null)
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                // Create the SolidDNA exception
                var error = new SolidDnaException(CreateError(errorTypeCode, errorCode, errorDescription), ex);

                // If it should just be logged and ignored, log it
                if (Logger.LogAndIgnoreUncaughtExceptions)
                {
                    // Log the error
                    Logger.LogCriticalSource($"SolidDNA Exception created. {error.SolidDnaError}");
                    if (error.InnerException != null)
                        Logger.LogCriticalSource($"Inner Exception: { error.InnerException.GetErrorMessage()}");
                }
                // Otherwise, throw 
                else
                    throw error;
            }
        }

        /// <summary>
        /// Runs a task and catches any exceptions thrown
        /// wrapping and rethrowing them as a <see cref="SolidDnaException"/>
        /// 
        /// Returns the result of the task
        /// </summary>
        /// <param name="task">The task to run</param>
        /// <param name="errorTypeCode">The <see cref="SolidDnaErrorTypeCode"/> to wrap the exception in</param>
        /// <param name="errorCode">The <see cref="SolidDnaErrorCode"/> to wrap the exception in</param>
        /// <param name="errorDescription">The description of the error if thrown. Gets localized before being thrown</param>
        public static async Task<T> WrapAwaitAsync<T>(Func<Task<T>> task, SolidDnaErrorTypeCode errorTypeCode, SolidDnaErrorCode errorCode, string errorDescription = null)
        {
            try
            {
                return await task();
            }
            catch (Exception ex)
            {
                // Create the SolidDNA exception
                var error = new SolidDnaException(CreateError(errorTypeCode, errorCode, errorDescription), ex);

                // If it should just be logged and ignored, log it
                if (Logger.LogAndIgnoreUncaughtExceptions)
                {
                    // Log the error
                    Logger.LogCriticalSource($"SolidDNA Exception created. {error.SolidDnaError}");
                    if (error.InnerException != null)
                        Logger.LogCriticalSource($"Inner Exception: { error.InnerException.GetErrorMessage()}");

                    // Return a default object
                    return default;
                }
                // Otherwise, throw it up
                throw error;
            }
        }


        #endregion
    }
}
