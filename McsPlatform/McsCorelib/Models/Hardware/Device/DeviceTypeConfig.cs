namespace McsCoreLib.Models.Hardware.Device
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class DeviceTypeConfig
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int TypeID { get; set; }

        [SugarColumn(IsNullable = true, Length = 500)]
        public string TypeName { get; set; }
    }
}