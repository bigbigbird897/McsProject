namespace CoreServiceLib.Models.PosCalConfig
{
    public class CalPosDef
    {
        public int PosID { get; set; } //标定点位置
        public string PosName { get; set; } //标定点名称
        public int PosTypeID { get; set; } //标定点类型
        public int ActionTypeID { get; set; }//动作类型编号
        public double PosCalX { get; set; } //标定点坐标X
        public double PosCalY { get; set; }//标定点坐标Y
        public double PosCalZ { get; set; }//标定点坐标Z
        public double PosCalR { get; set; }//标定点坐标R
        public int Handle { get; set; } //标定点手系(机械臂)
    }
}
