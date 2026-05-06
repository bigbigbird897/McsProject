using McsCoreLib.Paras;

using Newtonsoft.Json.Linq;


namespace McsCoreLib.Models.Workflow
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class WorkflowConfig
    {
        [SugarColumn(IsPrimaryKey = true, Length = 200, ColumnDescription = "流程编号")]
        public string WorkflowID { get; set; }

        [SugarColumn(Length = 200, ColumnDescription = "流程名称", IsNullable = true)]
        public string WorkflowName { get; set; }

        [SugarColumn(IsJson = true, ColumnDescription = "流程扩展参数")]
        public JObject OtherPara { get; set; }

        [SugarColumn(IsJson = true, ColumnDescription = "步序明细")]
        public List<StepInfo> Steps { get; set; } = [];

        [SugarColumn(ColumnDescription = "流程禁用")]
        public bool IsDisable { get; set; } = false;
    }
}