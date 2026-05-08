namespace CoreServiceLib.Core.Extensions.McsLogger
{
    public class MqttErrMessage
    {
        public string MessageID { get; set; }= "SysErrMessage";
        public SysErrMessage SysErrInfo { get; set; }
    }
}
