using CoreServiceLib.Models.Hardware.Device;
using CoreServiceLib.Paras;

using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Interfaces.Hardware
{
    public interface IDevice : IDisposable
    {
        /// <summary>
        /// 设备类型编号，设备驱动的唯一标识，开发时指定
        /// </summary>
        int DeviceTypeID { get; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        string DeviceTypeName { get; }

        string DiagUIName { get; }

        /// <summary>
        /// 设备状态
        /// </summary>
        DeviceStatusInfo DeviceStatus { get; }

        JObject DeviceConfigUI(JObject mcfg);

        void DeviceCreate(DeviceConfig mCfg);

        Func<IDevice,bool> SysInitAction { get; }

        Task DeviceOpenAsync(JObject mPara = null);

        Task DeviceCloseAsync(JObject mPara = null);

        Task<JObject> DeviceRunAsync();

        Task<JObject> DeviceRunAsync(JObject mPara);


        /// <summary>
        /// 设备停止退出
        /// </summary>
        /// <returns></returns>
        Task DeviceQuit();

        /// <summary>
        /// 设备急停
        /// </summary>
        /// <returns></returns>
        Task DeviceEstop();

        /// <summary>
        /// 设备复位
        /// </summary>
        /// <returns></returns>
        Task DeviceReset();

        Task TopicSubscribe(string topic);

        Task TopicUnsubscribe(string topic);

        Task PublishData(string topic, string message);

        Task<DeviceRevData> DeviceWriteAsync<T>(T mCmd, CancellationToken token = default);

        Task<byte[]> DeviceReadAsync<T>(T mCmd, CancellationToken token = default);

        string GetString(byte[] data, int startIndex, int mLength);

        byte GetByte(byte[] data, int startIndex);

        short GetShort(byte[] data, int startIndex);

        int GetInt(byte[] data, int startIndex);

        float GetFloat(byte[] data, int startIndex);

      

       
    }
}