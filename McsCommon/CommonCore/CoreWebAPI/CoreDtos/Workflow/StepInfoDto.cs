namespace CoreWebAPI.CoreDtos.Workflow
{
    /// <summary>
    /// 步骤配置信息
    /// </summary>
    public class StepInfoDto
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
        public string StepParaJson { get; set; }

        /// <summary>
        /// 步序禁用
        /// </summary>
        public bool IsDisable { get; set; }
    }
}
