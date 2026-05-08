namespace CoreWebAPI.CoreDtos.Material
{
    /// <summary>
    /// 上料
    /// </summary>
    public class LoadMarerialDto
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string TaskID { get; set; }
        /// <summary>
        ///设备区域编号
        /// </summary>
        public int DeviceAreaID { get; set; }
        /// <summary>
        /// 区域位置编号，由位置标定编号确定
        /// </summary>
        public int LoadMPosID { get; set; }
        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialID { get; set; }
        /// <summary>
        /// 剩余量/ 上料量
        /// </summary>
        public float Surplus { get; set; } 
    }
}
