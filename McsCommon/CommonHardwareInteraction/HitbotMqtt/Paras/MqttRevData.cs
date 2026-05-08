using CoreServiceLib.Paras;

namespace HitbotMqtt.Paras
{
    public class MqttRevData
    {
        public string ClientID { get; set; }
        public string Topic { get; set; }

        public DeviceRevData RevMessage { get; set; }

        public bool IsOK { get; set; }
        public bool IsRev { get; set; } = false; //是否接收反馈消息
    }
}