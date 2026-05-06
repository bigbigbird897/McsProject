namespace McsCoreLib.Models.Workflow
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class StepType
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "步骤类型编号")]
        public int StepTypeID { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "类型名称")]
        public string StepTypeName { get; set; }
    }
}