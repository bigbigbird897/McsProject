namespace McsCoreLib.Core.Extensions.McsLogger
{
    public class MqttLoggerMessage
    {
        /// <summary>
        /// 系统MQTT消息ID
        /// </summary>
        public string MessageID { get; set; } = "SysLogger";

        /// <summary>
        /// 系统日志列表
        /// </summary>
        public List<SysMessage> SysLoggers { get; set; }
    }
}
