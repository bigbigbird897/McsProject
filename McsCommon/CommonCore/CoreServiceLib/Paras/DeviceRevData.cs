using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Paras
{
    public class DeviceRevData
    {
        public int ResultCode { get; set; } = 1;
        public JObject Info { get; set; }
    }
}