using CoreServiceLib.Core.Extensions.McsLogger;

using Tcp.Interfaces;

using NLog;

using System.Buffers;
using System.Net;
using System.Net.Sockets;

using Volo.Abp.DependencyInjection;

namespace Tcp.Services
{
    public class McsTcpClient : IMcsTcpClient, ITransientDependency
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public async Task<byte[]> SendData(string mServerIP, int mPort, byte[] mSendData, bool isRev = true, int mTimeOut = 1000)
        {
            CancellationTokenSource tokenSource = new();
            List<byte> datas = [];
            var pool = ArrayPool<byte>.Shared;
            Socket mSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPAddress iPAddress = IPAddress.Parse(mServerIP);
                IPEndPoint serverEp = new(iPAddress, mPort);
                await mSocket.ConnectAsync(serverEp).ConfigureAwait(false);
                await mSocket.SendAsync(mSendData).ConfigureAwait(false);

                if (isRev)
                {
                    var buffer = pool.Rent(1024);
                    var mdelay = Task.Delay(mTimeOut);
                    var mrecv = Task.Run(async () =>
                    {
                        var counter = await mSocket.ReceiveAsync(buffer, SocketFlags.None, tokenSource.Token).ConfigureAwait(false);
                        datas.AddRange(buffer.Take(counter));
                    });

                    //超时等待
                    await Task.WhenAny(mdelay, mrecv);
                    if (mdelay.IsCompleted) tokenSource.Cancel();
                    pool.Return(buffer);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            finally
            {
                if (mSocket != null && mSocket.Connected)
                {
                    mSocket.Shutdown(SocketShutdown.Both);
                    mSocket.Close();
                }
                mSocket?.Dispose();
            }

            return [.. datas];
        }
    }
}