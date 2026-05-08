using HitbotMqtt.Paras;

namespace HitbotMqtt.Dtos
{
    public class HtMqttConfigDto
    {
        public string DeviceId { get; set; }
        public int DeviceTypeID { get; set; }
        public string LineID { get; set; }
        public string StationID { get; set; } = "";
        public bool IsDisable { get; set; }

        public HitbotMqttPara Para { get; set; }
    }
}