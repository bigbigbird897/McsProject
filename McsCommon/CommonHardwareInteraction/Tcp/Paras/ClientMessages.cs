using System.Net.Sockets;

namespace Tcp.Paras
{
    public class ClientMessages
    {
        public string ClientIp { get; set; }
        public Socket ClientSocket { get; set; }
        public byte[] ClientData { get; set; }
        public int DataLength { get; set; }
    }
}