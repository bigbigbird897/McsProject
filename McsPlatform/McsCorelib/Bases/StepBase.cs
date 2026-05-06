using McsCoreLib.Interfaces.Workflow;
using McsCoreLib.Paras;

using Newtonsoft.Json.Linq;

namespace McsCoreLib.Bases
{
    public abstract class StepBase : IStep
    {
        public abstract int StepTypeID { get; }
        public abstract string StepTypeName { get; }
        public virtual string StepConfigUIName { get; } = "";
        
        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="para">执行参数</param>
        /// <returns></returns>
        public virtual async Task<StepExcResult> RunAsync(JObject para)
        {
            await Task.CompletedTask;
            return new StepExcResult();
        }
    }
}
