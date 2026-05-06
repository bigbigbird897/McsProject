using McsCoreLib.Paras;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace McsCoreLib.Bases
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [McsApiGroup("系统配置")]
    [Authorize]
    public abstract class McsControlerBase : AbpControllerBase
    {
        protected McsWebApiResult SuccessResult()
        {
            return McsWebApiResult.SuccessResult();
        }

        protected McsWebApiResult<T> SuccessResult<T>(T mdata)
        {
            return McsWebApiResult<T>.SuccessResult(mdata);
        }

        protected McsWebApiResult FailResult(string msg = null)
        {
            return McsWebApiResult.FailResult(msg);
        }

        protected McsWebApiResult<T> FailResult<T>(string msg = null)
        {
            return McsWebApiResult<T>.FailResult(msg);
        }

        protected McsWebApiResult ErrorResult(string msg = null)
        {
            return McsWebApiResult.ErrorResult(msg);
        }

        protected McsWebApiResult<T> ErrorResult<T>(string msg = null)
        {
            return McsWebApiResult<T>.ErrorResult(msg);
        }

        protected McsWebApiResult Result(ResultStatus status, string msg = null)
        {
            return McsWebApiResult.Result(status, msg);
        }

        protected McsWebApiResult<T> Result<T>(ResultStatus status, T data, string msg = null)
        {
            return McsWebApiResult<T>.Result(status, data, msg);
        }
    }
}