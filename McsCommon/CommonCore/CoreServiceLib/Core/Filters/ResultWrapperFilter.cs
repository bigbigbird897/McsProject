using CoreServiceLib.Paras;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreServiceLib.Core.Filters
{
    public class ResultWrapperFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var controleractionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionWrapper = controleractionDescriptor?.MethodInfo.GetCustomAttributes(typeof(NoWrapperAttribute), false).FirstOrDefault();
            var controlerWrapper = controleractionDescriptor?.ControllerTypeInfo.GetCustomAttributes(typeof(NoWrapperAttribute), false).FirstOrDefault();
            if (actionWrapper != null || controlerWrapper != null) return;

            var rspResult = new McsWebApiResult();
            if (context.Result is ObjectResult)
            {
                var objresult = context.Result as ObjectResult;
                if (objresult?.Value == null)
                {
                    rspResult.Status = ResultStatus.Fail;
                    context.Result = new ObjectResult(rspResult);
                }
                else
                {
                    if (objresult.DeclaredType.IsGenericType &&
                        (objresult.DeclaredType?.GetGenericTypeDefinition() == typeof(McsWebApiResult<>)
                        || objresult.DeclaredType?.GetGenericTypeDefinition() == typeof(McsWebApiResult))) return;
                }
            }
            return;
        }
    }
}