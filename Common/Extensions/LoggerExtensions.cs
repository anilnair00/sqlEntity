using Microsoft.Extensions.Logging;

namespace AirCanada.Appx.Common.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogAndThrow<T>(this ILogger<T> logger, string className, string errorMessage, Exception ex)
        {
            logger.LogError(ex, "Error@{ClassName} => {ErrorMessage}", className, errorMessage);
            throw new InvalidOperationException($"Exception@{className} => {errorMessage}", ex);
        }

        public static void LogAndThrow<T>(this ILogger<T> logger, string className, string errorMessage)
        {
            logger.LogError("Error@{ClassName} => {ErrorMessage}", className, errorMessage);
            throw new InvalidOperationException($"Exception@{className} => {errorMessage}");
        }
    }
}