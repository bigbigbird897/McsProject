namespace CoreServiceLib.Models.PosCalConfig
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class PosCalData
    {
        public int AreaPosID { get; set; }//区域内位置编号
        public string PosName { get; set; }//区域位置名称
        public int ActionTypeID { get; set; }//动作类型编号
        public double X { get; set; }//区域位置坐标X
        public double Y { get; set; }//区域位置坐标Y
        public double Z { get; set; }//区域位置坐标Z
        public double R { get; set; }//区域旋转角度R
        public int Handle { get; set; }//坐标手系
        public double OffsetX { get; set; }//区域位置坐标X补偿
        public double OffsetY { get; set; }//区域位置坐标Y补偿
        public double OffsetZ { get; set; }//区域位置坐标Z补偿
        public double OffsetR { get; set; }//区域位置坐标R补偿
    }
}
