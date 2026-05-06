namespace McsCoreLib.Models.Hardware
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class StationFunType
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int FuncTypeID { get; set; }

        [SugarColumn(Length = 200)]
        public string FuncTypeName { get; set; }
    }
}