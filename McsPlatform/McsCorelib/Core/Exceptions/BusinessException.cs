using System.Net;

namespace McsCoreLib.Core.Exceptions
{
    public class BusinessException(HttpStatusCode code, string message) : Exception(message)
    {
        public HttpStatusCode Code { get; private set; } = code;
        public new string Message { get; private set; } = message;
    }
}