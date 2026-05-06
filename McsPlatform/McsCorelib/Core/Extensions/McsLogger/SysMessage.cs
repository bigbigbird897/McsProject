namespace McsCoreLib.Core.Extensions.McsLogger
{
    public class SysMessage
    {
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime OccurredTime { get; set; }

        //模块名称
        public string ModeName { get; set; }
        //日志类型
        public LoggerInfoType MType { get; set; }

        //日志信息
        public string MessageInfo { get; set; }
    }
}