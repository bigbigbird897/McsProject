namespace CoreServiceLib.Tools
{
    public class CrcTool
    {
        /// <summary>
        /// CRC16_Modbus效验 , bigHead == 高(H) 在前
        /// </summary>
        /// <param name="byteData">要进行计算的字节数组</param>
        /// <returns>计算后的数组</returns>
        public static byte[] ToModbusCrcData(byte[] byteData, bool mIsOnlyRetrunCrc = false, bool mIsBigHead = true)
        {
            List<byte> result = [];
            byte[] CRC = new byte[2];
            if (!mIsOnlyRetrunCrc) result.AddRange(byteData);
            ushort wCrc = 0xFFFF;
            for (int i = 0; i < byteData.Length; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((wCrc & 0x0001) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;//异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }
            if (mIsBigHead)
            {
                CRC[0] = (byte)((wCrc & 0xFF00) >> 8);//高位在前
                CRC[1] = (byte)(wCrc & 0x00FF);       //低位在后
            }
            else
            {
                CRC[0] = (byte)wCrc; //低位在前
                CRC[1] = (byte)(wCrc >> 8); //高位在后
            }

            result.AddRange(CRC);
            return [.. result];
        }

        public static byte[] Int16ToBytes(short data, bool isBigHead = true)
        {
            var datas = BitConverter.GetBytes(data);
            if (isBigHead) Array.Reverse(datas);
            return datas;
        }

        public static double BytesToDouble(byte[] buffers, int startIndex)
        {
            double x = 0;
            if (buffers.Length > startIndex)
            {
                var length = buffers[startIndex];
                startIndex++;
                var endIndex = startIndex + length;
                var datas = buffers[startIndex..endIndex];
                x = BitConverter.ToDouble(datas, 0);
            }
            return x;
        }

        public static float BytesToFloat(byte[] buffers, int startIndex, bool isReverse = false)
        {
            float x = 0;
            if (buffers.Length > startIndex)
            {
                var length = buffers[startIndex];
                startIndex++;
                var endIndex = startIndex + length;
                var datas = buffers[startIndex..endIndex];
                if (isReverse) Array.Reverse(datas);
                x = BitConverter.ToSingle(datas, 0);
            }

            return x;
        }

        public static short BytesToIn16(byte[] buffers, int startIndex, bool isReverse = false)
        {
            short x = 0;
            if (buffers.Length > startIndex)
            {
                var length = buffers[startIndex];
                startIndex++;
                var endIndex = startIndex + length;
                var datas = buffers[startIndex..endIndex];
                if (isReverse) Array.Reverse(datas);
                x = BitConverter.ToInt16(datas, 0);
            }
            return x;
        }

        public static int BytesToIn32(byte[] buffers, int startIndex, bool isReverse = false)
        {
            int x = 0;
            if (buffers.Length > startIndex)
            {
                var length = buffers[startIndex];
                startIndex++;
                var endIndex = startIndex + length;
                var datas = buffers[startIndex..endIndex];
                if (isReverse) Array.Reverse(datas);
                x = BitConverter.ToInt32(datas, 0);
            }
            return x;
        }

        public static short BytesToIn16_2(byte[] buffers, int startIndex, bool isReverse = false)
        {
            short x = 0;
            if (buffers.Length > startIndex)
            {
                var endIndex = startIndex+2 ;
                var datas = buffers[startIndex..endIndex];
                if (isReverse) Array.Reverse(datas);
                x = BitConverter.ToInt16(datas, 0);
            }
            return x;
        }
    }
}