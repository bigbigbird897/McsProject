using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Paras
{
    public class StepExcResult
    {
        /// <summary>
        /// 反馈结果
        /// </summary>
        public bool IsOk { get; set; } =true;
        /// <summary>
        ///  反馈消息
        /// </summary>  
        public JObject ResultData { get; set; } = [];
    }
}
