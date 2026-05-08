namespace CoreWebAPI.CoreDtos.Manuls
{
    /// <summary>
    ///  手动控制返回数据
    /// </summary>
    /// <typeparam name="TitlePara">标题参数</typeparam>
    /// <typeparam name="ActionPara">动作参数</typeparam>
    /// <typeparam name="OutputPara">输出显示参数</typeparam>
    public class ManulRevDataDto<TitlePara, ActionPara, OutputPara>
    {
        /// <summary>
        /// 记录编号- 自增长
        /// </summary>
        public int MID { get; }

        /// <summary>
        /// 控件类型
        /// </summary>
        public int ManulTypeID { get; set; }

        /// <summary>
        /// 控件名称
        /// </summary>
        public string ManulControlName { get; set; }

        /// <summary>
        /// 页编号
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 当前页行号
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// 当前页列号
        /// </summary>
        public int ColNumber { get; set; }

        /// <summary>
        /// 标题参数
        /// </summary>
        public TitlePara ManulTitles { get; set; }

        /// <summary>
        /// 动作参数
        /// </summary>
        public ActionPara ManulActionParas { get; set; }

        /// <summary>
        /// 输出参数
        /// </summary>
        public OutputPara OutputParas { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool IsDisable { get; set; }
    }
}
