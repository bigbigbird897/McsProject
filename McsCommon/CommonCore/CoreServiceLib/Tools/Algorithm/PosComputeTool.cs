using System.Text;

namespace CoreServiceLib.Tools.Algorithm
{
    public class PosComputeTool
    {
        /// <summary>
        ///  4 点标定获取(X,Y)坐标值
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="mColumnXCounter"></param>
        /// <param name="mRowYCounter"></param>
        /// <returns></returns>
        public static CalPosData[] GetPosCalPosDatas(Pos p1, Pos p2, Pos p3, Pos p4, int mColumnXCounter, int mRowYCounter)
        {
            List<CalPosData> result = [];
            int asc = 'A';
            for (int y = 0; y < mRowYCounter; y++)
            {
                var name = Encoding.ASCII.GetString([(byte)(asc + y)]);
                for (int x = 0; x < mColumnXCounter; x++)
                {
                    var x1 = p1.X + (p2.X - p1.X) * x / (mColumnXCounter - 1);
                    var xn = x1 + (p3.X + (p4.X - p3.X) * x / (mColumnXCounter - 1) - x1) * y / (mRowYCounter - 1);
                    var y1 = p1.Y + (p3.Y - p1.Y) * y / (mRowYCounter - 1);
                    var yn = y1 + (p2.Y + (p4.Y - p2.Y) * y / (mRowYCounter - 1) - y1) * x / (mColumnXCounter - 1);
                    result.Add(new CalPosData() { PosID = mColumnXCounter * y + x + 1, PosName = $"{name}{x + 1}", Data = new Pos { X = xn, Y = yn } });
                }
            }

            return [.. result];
        }

        /// <summary>
        /// 依据行列计算位置编号
        /// </summary>
        /// <param name="mColumnXCounter"></param>
        /// <param name="mRowYCounter"></param>
        /// <returns></returns>
        public static int[] GetPosIDs(int mColumnXCounter, int mRowYCounter)
        {
            List<int> result = [];
            for (int y = 0; y < mRowYCounter; y++)
            {
                for (int x = 0; x < mColumnXCounter; x++)
                {
                    result.Add(mColumnXCounter * y + x + 1);
                }
            }
            return [.. result];
        }


        public static (int posid, string posName)[] GetPosIDAndNams(int mColumnXCounter, int mRowYCounter)
        {
            List<(int posid, string posName)> result = [];
            int asc = 'A';
            for (int y = 0; y < mRowYCounter; y++)
            {
                var name = Encoding.ASCII.GetString([(byte)(asc + y)]);
                for (int x = 0; x < mColumnXCounter; x++)
                {
                    result.Add((mColumnXCounter * y + x + 1, $"{name}{x + 1}"));
                }
            }

            return [.. result];
        }

        public static int GetPosID(string mPosName)
        {         
            var m = mPosName.ToUpper();
            var chararray = m.ToCharArray();
            int c = chararray[0] switch
            {
                'A' => 0,
                'B' => 1,
                'C' => 2,
                'D' => 3,
                'E' => 4,
                'F' => 5,
                'G' => 6,
                'H' => 7,
                _ => -1
            };

            var x= chararray.Skip(1).ToArray();
            int r =int.Parse(new string(x));
            var id = c*12 + r;
            return id;
        }

        public static string GetPlateID(int mId)
        {
            var c= (mId/13) switch
            {
                0 =>'A',
                1 =>'B',
                2 =>'C',
                3 =>'D',
                4=>'E',
                5=>'F',
                6=>'G',
                7=>'H',
                _=>'0'
            };
            var x= mId % 12;
            x=x==0 ? 12 : x;
            return $"{c}{x}";
        }


        /// <summary>
        /// 慧灵手臂位置解析
        /// </summary>
        /// <param name="mPos"></param>
        /// <returns></returns>
        public static HRobotPos GetPobotPos(string mPos)
        {
            if (string.IsNullOrEmpty(mPos)) return null;
            var datas = mPos.Split(',');
            if (datas.Length < 5) return null;
            return new HRobotPos()
            {
                X = float.Parse(datas[0]),
                Y = float.Parse(datas[1]),
                Z = float.Parse(datas[2]),
                R = float.Parse(datas[3]),
                Handle = int.Parse(datas[4])
            };
        }
    }

    public class Pos
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class CalPosData
    {
        public int PosID { get; set; }
        public string PosName { get; set; }
        public Pos Data { get; set; }
    }

    public class HRobotPos
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float R { get; set; }
        public int Handle { get; set; }
    }


}