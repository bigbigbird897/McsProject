namespace McsCoreInterface.CoreDtos.Workflow
{
    /// <summary>
    /// 命令类型
    /// </summary>
    public class CmdTypeDto
    {
        /// <summary>
        /// 命令类型编号
        /// </summary>
        public int StepTypeID { get; set; }
        /// <summary>
        /// 命令类型名称 --下拉选择呈现
        /// </summary>
        public string StepTypeName { get; set; }

    }
}
