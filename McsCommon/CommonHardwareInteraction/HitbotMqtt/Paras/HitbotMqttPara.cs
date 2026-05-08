namespace HitbotMqtt.Paras
{
    public class HitbotMqttPara
    {
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public string UserID { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsUsePassword { get; set; } = false;
        public int TimeOut { get; set; }
        public string ClientID { get; set; }

        public string MqttSysMessageTopic { get; set; }
    }
}