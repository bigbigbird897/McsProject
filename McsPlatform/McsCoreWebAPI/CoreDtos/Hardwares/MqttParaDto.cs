using System.ComponentModel;

namespace McsCoreInterface.CoreDtos.Hardwares
{
    /// <summary>
    /// MQTT配置参数
    /// </summary>
    public class MqttParaDto
    {
        /// <summary>
        /// 硬件编号
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [DefaultValue("中央控制器")]
        public string DeviceName { get; set; }

        /// <summary>
        /// mqtt IP 地址
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// Mqtt 连接端口
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 设置通信超时时间(ms)
        /// </summary>
        [DefaultValue(5000)]
        public int TimeOut { get; set; }

        /// <summary>
        /// 客户端编号
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// 订阅平台推送系统消息
        /// </summary>
        public string MqttSysMessageTopic { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        [DefaultValue(false)]
        public bool IsDisable { get; set; }
    }
}