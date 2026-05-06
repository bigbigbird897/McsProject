using Newtonsoft.Json.Linq;

namespace McsCoreLib.Models.Hardware.Device
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class DeviceConfig
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "设备编号")]
        public string DeviceId { get; set; }

        [SugarColumn(ColumnDescription = "设备名称", Length = 200)]
        public string DeviceName { get; set; }

        [SugarColumn(ColumnDescription = "设备类型编号")]
        public int DeviceTypeID { get; set; }

        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "产线编号")]
        public string LineID { get; set; }

        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDescription = "工位编号")]
        public string StationID { get; set; } = "";

        [SugarColumn(IsJson = true, ColumnDescription = "设备配置参数")]
        public JObject DevicePara { get; set; }

        public bool IsDisable { get; set; }
    }
}