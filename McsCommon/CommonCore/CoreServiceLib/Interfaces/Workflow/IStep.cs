using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Paras;

using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Interfaces.Workflow
{
    public interface IStep
    {
        /// <summary>
        /// 步序类型编号
        /// </summary>
        int StepTypeID { get; }

        /// <summary>
        /// 步序类型名称
        /// </summary>
        string StepTypeName { get; }

        /// <summary>
        /// 配置UI名称
        /// </summary>
        string StepConfigUIName { get; }

        /// <summary>
        /// 解析步序参数
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<StepExcResult> RunAsync(JObject para);

        /// <summary>
        /// 参数配置界面
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        JObject OpenConfingUI(JObject para)
        {
            para ??= [];
            var dig = McsApp.GetDiag(StepConfigUIName, para);
            dig?.ShowDialogUI();
            return McsApp.GetParaData<JObject>();
        }
    }
}