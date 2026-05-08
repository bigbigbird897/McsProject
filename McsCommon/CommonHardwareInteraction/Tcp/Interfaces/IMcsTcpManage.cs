namespace Tcp.Interfaces
{
    public interface IMcsTcpManage
    {
        /// <summary>
        /// 本地TCP服务器启动
        /// </summary>
        void TcpServerRun();

        /// <summary>
        ///  本地TCP服务器停止
        /// </summary>
        void TcpServerStop();

        /// <summary>
        ///  依据端口获取本地已启用的TCP服务器
        /// </summary>
        /// <param name="mPort">本地服务器端口号</param>
        /// <returns></returns>
        IMcsTcpServer GetServer(int mPort);

        /// <summary>
        /// 向指定TCP服务器发送消息
        /// </summary>
        /// <param name="mServerIP">TCP 服务器 IP</param>
        /// <param name="mPort">TCP 服务器 端口</param>
        /// <param name="mSendData">发送数据</param>
        /// <param name="isRev">是否等待反馈，默认是</param>
        /// <param name="mTimeOut">通信超时时间，默认1S</param>
        /// <returns></returns>
        Task<byte[]> SendServerDataAsync(string mServerIP, int mPort, byte[] mSendData, bool isRev = true, int mTimeOut = 1000);
    }
}