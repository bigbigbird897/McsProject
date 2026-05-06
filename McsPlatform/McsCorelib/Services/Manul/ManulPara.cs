namespace McsCoreLib.Services.Manul
{
    public class ManulPara<Title, TActionPara, TOutputPara>
    
    {
        /// <summary>
        /// 记录编号 --- 自动生成
        /// </summary>
        public int MID { get; set; }
        public int ManulTypeID { get; set; }
        public string ManulControlName { get; set; }
        public int PageNumber { get; set; }
        public int RowNumber { get; set; }
        public int ColNumber { get; set; }
        public string DeviceId { get; set; }
        public Title ManulTitles { get; set; }
        public TActionPara ManulActionPara { get; set; }
        public TOutputPara OutputParas { get; set; }

        public bool IsDisable { get; set; }
    }
}
