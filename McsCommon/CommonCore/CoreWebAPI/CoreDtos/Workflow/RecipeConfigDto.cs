using Newtonsoft.Json.Linq;

namespace CoreWebAPI.CoreDtos.Workflow
{
    /// <summary>
    /// 实验流程
    /// </summary>
    public class RecipeConfigDto
    {
        /// <summary>
        /// 流程编号 -- 自动创建，可修改。呈现隐藏
        /// </summary>
        public string WorkflowID { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        public string WorkflowName { get; set; }

        /// <summary>
        /// 流程其他参数
        /// </summary>
        public JObject OtherPara { get; set; }

        /// <summary>
        /// 流程步序集
        /// </summary>
        public List<StepInfoDto> Steps { get; set; } = [];
    }
}
