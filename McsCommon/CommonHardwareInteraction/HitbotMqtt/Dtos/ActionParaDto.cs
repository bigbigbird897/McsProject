using HitbotMqtt.Messages;

namespace HitbotMqtt.Dtos
{
    public class ActionParaDto
    {
        /// <summary>
        /// pause = 暂停; resume= 恢复; stop=停止
        /// </summary>
        public string action { get; set; }
    }

    public class McsAction:SetParaBase
    {
        /// <summary>
        /// # 1:运行，2：暂停，3终止
        /// </summary>
        public int SysControl { get; set; }
    }
}
