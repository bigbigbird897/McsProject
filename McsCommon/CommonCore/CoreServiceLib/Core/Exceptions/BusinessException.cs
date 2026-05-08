using System.Net;

namespace CoreServiceLib.Core.Exceptions
{
    public class BusinessException(HttpStatusCode code, string message) : Exception(message)
    {
        public HttpStatusCode Code { get; private set; } = code;
        public new string Message { get; private set; } = message;
    }
}