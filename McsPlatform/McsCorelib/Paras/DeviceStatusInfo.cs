using Newtonsoft.Json.Linq;

namespace McsCoreLib.Paras
{
    public class DeviceStatusInfo
    {
        public DeviceStatusEnum DeviceStatus { get; set; }

        /// <summary>
        /// 设备状态信息
        /// </summary>
        public JObject Message { get; set; }
    }

    public enum DeviceStatusEnum
    {
        Unknown = 0,
        Wait = 1,
        Run = 2,
        Error = 3
    }
}