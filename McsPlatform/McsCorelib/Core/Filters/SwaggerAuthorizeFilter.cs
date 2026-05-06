using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace McsCoreLib.Core.Filters
{
    /// <summary>
    /// 在Swagger文档中给需要Authorize的接口添加🔒
    /// </summary>

    public sealed class SwaggerAuthorizeFilter : IOperationFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                                        .Union(context.MethodInfo.GetCustomAttributes(true))
                                        .OfType<AuthorizeAttribute>();
            if (!authAttributes!.Any()) return;

            operation.Security ??= [];
            operation.Security.Add(new ()
            {
                [new("JWTBearer", context.Document)] = []
            });
            operation.Responses ??= [];
            operation.Responses.Add("401", new OpenApiResponse() { Description = "Unauthorized" });
        }
    }
}