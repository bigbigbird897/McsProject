using System.Collections.Concurrent;

namespace CoreServiceLib.Core.Extensions.McsLogger
{
    public static class LogBuffer
    {
        //界面显示用缓存
        private static readonly ConcurrentQueue<SysMessage> _LoggerBuffer = new();

        //界面显示缓存大小， 默认100个字符串
        public static int BufferSize { get; set; } = 100;

        /// <summary>
        ///  更新显示缓存数据
        /// </summary>
        /// <param name="message"></param>
        public static List<SysMessage> SendBuffer(SysMessage message)
        {
            _LoggerBuffer.Enqueue(message);

            var size = _LoggerBuffer.Count - BufferSize;
            BufferDequeue(size);
            return GetBufferData();
        }

        public static void ResetBuffer()
        {
            BufferDequeue(_LoggerBuffer.Count);
        }

        /// <summary>
        ///  获取显示缓存数据
        /// </summary>
        /// <returns></returns>
        public static List<SysMessage> GetBufferData()
        {
            List<SysMessage> data = [.. _LoggerBuffer];
            data.Reverse();
            return data;
        }

        private static List<SysMessage> BufferDequeue(int size)
        {
            List<SysMessage> datas = [];
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    _LoggerBuffer.TryDequeue(out SysMessage impData);
                    if (impData != null)
                        datas.Add(impData);
                }
            }
            return datas;
        }
    }
}