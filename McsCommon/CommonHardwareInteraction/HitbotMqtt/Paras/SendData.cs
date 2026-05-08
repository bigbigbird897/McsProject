namespace HitbotMqtt.Paras
{
    public class SendData(string mSendMessage, int mRevTimeOut = 10000)
    {
        //发送消息主题标识
        public string SendTopic { get; set; } = @"/hitbot/cyys/action/send/";

        //等待反馈
        public bool IsWaitRev { get; set; } = true;

        //等待反馈的主题标识
        public string RevTopic { get; set; } = "SysStatus";

        //反馈超时时间
        public int RevTimeOut { get; set; } = mRevTimeOut;

        //反馈消息轮询时间(默认50ms)
        public int RevScanTime { get; set; } = 50;

        /// <summary>
        /// 下发消息
        /// </summary>
        public string SendMessage { get; set; } = mSendMessage;
    }
}