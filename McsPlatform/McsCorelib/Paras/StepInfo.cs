using Newtonsoft.Json.Linq;

namespace McsCoreLib.Paras
{
    public class StepInfo
    {
        /// <summary>
        /// 步骤编号(命令)
        /// </summary>
        public int StepID { get; set; }

        /// <summary>
        /// 步骤名称
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 步骤类型ID
        /// </summary>
        public int StepTypeID { get; set; }

        /// <summary>
        /// 步骤参数
        /// </summary>
        public JObject StepPara { get; set; }

        /// <summary>
        /// 步序禁用
        /// </summary>
        public bool IsDisable { get; set; }
    }
}