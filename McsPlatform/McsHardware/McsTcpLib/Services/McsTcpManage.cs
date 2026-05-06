using McsCoreLib.Core.IocExt;

using McsTcpLib.Interfaces;

using Microsoft.Extensions.Configuration;

using NLog;

using Volo.Abp.DependencyInjection;

namespace McsTcpLib.Services
{
    public class McsTcpManage(IConfiguration mConfig) : IMcsTcpManage, ISingletonDependency
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, IMcsTcpServer> mTcpServerList = [];

        public IMcsTcpServer GetServer(int mPort)
        {
            return mTcpServerList[mPort];
        }

        /// <summary>
        /// 向指定TCP服务器发送消息
        /// </summary>
        /// <param name="mServerIP">服务器地址</param>
        /// <param name="mPort"></param>
        /// <param name="mSendData"></param>
        /// <param name="isRev"></param>
        /// <param name="mTimeOut"></param>

        public Task<byte[]> SendServerDataAsync(string mServerIP, int mPort, byte[] mSendData, bool isRev = true, int mTimeOut = 1000)
        {
            var client = McsApp.McsServiceProvider.Resolve<IMcsTcpClient>();
            var datas = client.SendData(mServerIP, mPort, mSendData, isRev, mTimeOut);
            return datas;
        }

        public void TcpServerRun()
        {
            try
            {
                // 依据配置可启用多个端口的Tcp 服务
                var port = int.Parse(mConfig["TcpServer:Port"]);
                IMcsTcpServer mcsTcpServer = McsApp.McsServiceProvider.Resolve<IMcsTcpServer>();
                mcsTcpServer.TcpServerInit(port);
                mcsTcpServer.ServerOn();
                mTcpServerList.Add(port, mcsTcpServer);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void TcpServerStop()
        {
            foreach (var item in mTcpServerList)
            {
                item.Value.ServerOff();
            }
        }
    }
}