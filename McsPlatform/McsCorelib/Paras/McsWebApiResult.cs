using McsCoreLib.Core.Extensions;

using System.ComponentModel;

namespace McsCoreLib.Paras
{
    public class McsWebApiResult
    {
        public int Code { get; set; }
        public ResultStatus Status { get; set; }

        private string _msg;

        public string Message
        {
            get => !string.IsNullOrEmpty(_msg) ? _msg : Status.ToDescription();
            set { _msg = value; }
        }

        public static McsWebApiResult SuccessResult()
        {
            return new McsWebApiResult() { Status = ResultStatus.Success, Code = 0 };
        }

        public static McsWebApiResult FailResult(string message = null)
        {
            return new McsWebApiResult() { Status = ResultStatus.Fail, Message = message, Code = (int)ResultStatus.Fail };
        }

        public static McsWebApiResult ErrorResult(string message = null)
        {
            return new McsWebApiResult() { Status = ResultStatus.Error, Message = message, Code = (int)ResultStatus.Error };
        }

        public static McsWebApiResult Result(ResultStatus status, string message = null)
        {
            return new McsWebApiResult() { Status = status, Message = message, Code = (int)status };
        }
    }

    public class McsWebApiResult<T> : McsWebApiResult
    {
        public T Data { get; set; }

        public static McsWebApiResult<T> SuccessResult(T data)
        {
            return new McsWebApiResult<T>() { Status = ResultStatus.Success, Data = data, Code = 0 };
        }

        public new static McsWebApiResult<T> FailResult(string message = null)
        {
            return new McsWebApiResult<T>() { Status = ResultStatus.Fail, Message = message, Code = (int)ResultStatus.Fail };
        }

        public new static McsWebApiResult<T> ErrorResult(string message = null)
        {
            return new McsWebApiResult<T>() { Status = ResultStatus.Error, Message = message, Code = (int)ResultStatus.Error };
        }

        public static McsWebApiResult<T> Result(ResultStatus status, T data, string message = null)
        {
            return new McsWebApiResult<T>() { Status = status, Message = message, Data = data, Code = (int)status };
        }

        //隐式将T 转换为 WebApiResult<T>
        public static implicit operator McsWebApiResult<T>(T data)
        {
            return new McsWebApiResult<T> { Data = data };
        }
    }

    public enum ResultStatus
    {
        [Description("请求成功")]
        Success = 0,

        [Description("请求失败")]
        Fail = 1,

        [Description("请求异常")]
        Error = 2
    }
}