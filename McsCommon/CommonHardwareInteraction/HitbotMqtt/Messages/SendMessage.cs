using HitbotMqtt.Messages;

using System.Text.Json;

namespace HitbotMQTT.Messages
{
    public class SendMessage<T> where T : SetParaBase
    {
        public string message_type { get; set; } //固定为:request
        public string message_id { get; set; } //消息的唯一标识
        public string instruct { get; set; } //执行流程图的块名称

        public T set { get; set; } // 变量名
        public Status status { get; set; } //系统状态

        public SendMessage()
        {
            status = new Status()
            {
                alarm_mes = "",
                work = 0
            };

            message_id = Guid.NewGuid().ToString();
            message_type = "request";
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}