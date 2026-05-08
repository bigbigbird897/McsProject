using CoreServiceLib.Core.Extensions.McsLogger;
using CoreServiceLib.Core.McsEvent;

using Tcp.Interfaces;
using Tcp.Paras;

using NLog;

using System.Buffers;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Volo.Abp.DependencyInjection;

namespace Tcp.Services
{
    public class McsTcpServer : IMcsTcpServer, ITransientDependency, IDisposable
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private bool disposedValue = false;
        private Socket mServerSocket;

        private readonly ConcurrentDictionary<string, (Socket client, CancellationTokenSource mtoken)> mClients = [];

        //监听取消
        private readonly CancellationTokenSource mCancellToken = new();

        public bool IsRuning { get; private set; }

        public IPAddress Address { get; private set; }

        public int Port { get; private set; }

        public List<string> ClientList => [.. mClients.Keys];

        /// <summary>
        /// 加载
        /// </summary>

        public void TcpServerInit(int mPort)
        {
            try
            {
                IsRuning = false;
                AsyncTcpServer(mPort);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        /// <summary>
        /// 服务启动
        /// </summary>
        /// <returns></returns>

        public void ServerOn()
        {
            if (!IsRuning)
            {
                mServerSocket.Listen();

                IsRuning = true;
                _ = AcceptClientAsync().ConfigureAwait(false);
            }
        }

        public async Task SendMessage(string mclientIP, byte[] mdata)
        {
            try
            {
                var client = mClients[mclientIP];
                await client.client.SendAsync(mdata, client.mtoken.Token);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <returns></returns>

        public void ServerOff()
        {
            mCancellToken.Cancel();
            IsRuning = false;
            if (ClientList.Count != 0) mServerSocket?.Shutdown(SocketShutdown.Both);
            mServerSocket?.Close();
            mCancellToken.Dispose();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ServerOff();
                }
                disposedValue = true;
            }
        }

        private void AsyncTcpServer(int listenPort)
        {
            Port = listenPort;
            mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Address = IPAddress.Any;
            mServerSocket.Bind(new IPEndPoint(Address, listenPort));
        }

        private async Task AcceptClientAsync()
        {
            //限流
            var smeaphore = new SemaphoreSlim(200);
            while (IsRuning)
            {
                await smeaphore.WaitAsync();
                //监听请求
                var clientsocket = await mServerSocket.AcceptAsync(mCancellToken.Token);

                //去除客户端端口号
                var ip = clientsocket.RemoteEndPoint.ToString().Split(':')[0];
                _ = HandleClientAsync(clientsocket, ip).ContinueWith(_ => smeaphore.Release());
            }
        }

        private async Task HandleClientAsync(Socket clientSocket, string ip)
        {
            // 使用ArrayPool共享缓冲区
            var pool = ArrayPool<byte>.Shared;
            CancellationTokenSource cts = new();
            try
            {
                logger.Info($"客户端-{ip}已连接");
                //释放已异常中断的连接
                var m = mClients.Keys.Where(a => a == ip).FirstOrDefault();
                if (!string.IsNullOrEmpty(m))
                {
                    mClients[m].mtoken.Cancel();
                    mClients.Remove(m, out var d);
                }
                mClients.TryAdd(ip, (clientSocket, cts));
                while (IsRuning && !cts.Token.IsCancellationRequested)
                {
                    var buffer = pool.Rent(4096);
                    int received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None, cts.Token);
                    if (received > 0)
                    {
                        //发布消息
                        logger.Info($"接收客户端-{ip}-消息: {Encoding.ASCII.GetString(buffer, 0, received)}");
                        McsEventMessage.Instance.Publish(new ClientMessages()
                        {
                            ClientIp = $"{clientSocket.RemoteEndPoint}",
                            ClientData = buffer,
                            ClientSocket = clientSocket,
                            DataLength = received
                        });
                        pool.Return(buffer);
                    }
                    else
                    {
                        pool.Return(buffer);
                        break;
                    }
                }
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
            {
                logger.Error(ex);
            }
            catch (ObjectDisposedException ex)
            {
                logger.Error(ex);
            }
            finally
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
                clientSocket?.Dispose();
                mClients.Remove(ip, out var d);
                logger.Info($"客户端-{ip}已关闭");
            }
        }
    }
}