using System.Net;

namespace McsTcpLib.Interfaces
{
    public interface IMcsTcpServer
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        bool IsRuning { get; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        IPAddress Address { get; }

        /// <summary>
        /// 服务器监听端口
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 服务器初始化
        /// </summary>
        void TcpServerInit(int mPort);

        /// <summary>
        /// 服务器启动
        /// </summary>
        /// <returns></returns>
        void ServerOn();

        /// <summary>
        /// 服务器关闭
        /// </summary>
        /// <returns></returns>
        void ServerOff();

        /// <summary>
        /// 向指定客户端发送消息
        /// </summary>
        /// <param name="mclientIP">客户端IP</param>
        /// <param name="mdata"></param>
        /// <returns></returns>
        Task SendMessage(string mclientIP, byte[] mdata);

        /// <summary>
        /// 获取客户端列表
        /// </summary>
        /// <returns></returns>
        List<string> ClientList { get; }
    }
}