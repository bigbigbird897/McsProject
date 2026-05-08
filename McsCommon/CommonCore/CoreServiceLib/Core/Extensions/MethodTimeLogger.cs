using System.Reflection;

namespace CoreServiceLib.Core.Extensions
{
    public static class MethodTimeLogger
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Log(MethodBase methodBase, TimeSpan elapsed, string message)
        {
            _logger.Debug($"Method: {methodBase.Name} Elapsed: {elapsed.TotalMilliseconds}ms Message: {message}");
        }
    }
}