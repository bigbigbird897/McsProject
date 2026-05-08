using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreServiceLib.Core.Exceptions
{
    /// <summary>
    ///     <inheritdoc cref="IExceptionHandler" />
    /// </summary>
    /// <param name="environment"></param>
    public sealed class BusinessExceptionHandler(IHostEnvironment environment) : IExceptionHandler
    {
        /// <inheritdoc cref="IExceptionHandler.TryHandleAsync" />
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // 当不是 BusinessException 的时候本 Handler 不处理
            if (exception is not BusinessException business) return false;
            var details = new ProblemDetails
            {
                Status = (int?)business.Code,
                Title = business.Message
            };
            // 当为开发环境的时候,可以输出一些详细的信息.
            if (environment.IsDevelopment())
            {
                details.Detail = $"""
                              {business.Source}
                              {business.StackTrace}
                              """;
            }
            httpContext.Response.StatusCode = details.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
            return true;
        }
    }
}