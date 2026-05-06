using System.ComponentModel;

namespace McsCoreInterface.CoreMessages
{
    /// <summary>
    /// 主窗口控制
    /// </summary>
    public class ChangeWindow
    {
        /// <summary>
        /// 窗口控制
        /// </summary>
        public ChangeType ChageM { get; set; }
    }

    /// <summary>
    ///  控制类型
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        ///  最小化
        /// </summary>
        [Description("窗口最小化")]
        Min,

        /// <summary>
        /// 最大化
        /// </summary>
        [Description("窗口最大化")]
        Max,

        /// <summary>
        /// 关闭
        /// </summary>
        [Description("窗口关闭")]
        Close
    }
}