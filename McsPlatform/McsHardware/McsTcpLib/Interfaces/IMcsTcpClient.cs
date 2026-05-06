namespace McsTcpLib.Interfaces
{
    public interface IMcsTcpClient
    {
        Task<byte[]> SendData(string mServerIP, int mPort, byte[] mSendData, bool isRev = true, int mTimeOut = 1000);
    }
}