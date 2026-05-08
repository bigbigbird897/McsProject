using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Interfaces.Hardware;
using CoreServiceLib.Models.Hardware.Device;
using CoreServiceLib.Paras;

using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Bases
{
    public abstract class DeviceBase : IDevice
    {
        public abstract int DeviceTypeID { get; }
        public abstract string DeviceTypeName { get; }

        public abstract string DiagUIName { get; }

        public JObject DeviceConfigUI(JObject mcfg)
        {
            mcfg ??= [];
            var dig = McsApp.GetDiag(DiagUIName, mcfg);
            dig.ShowDialogUI();
            return dig.ReturnData<JObject>();
        }

        public virtual DeviceStatusInfo DeviceStatus { get; }

        public virtual Func<IDevice, bool> SysInitAction { get; }

        public abstract void DeviceCreate(DeviceConfig mCfg);

        public abstract Task DeviceQuit();

        public virtual async Task<DeviceRevData> DeviceWriteAsync<T>(T mCmd, CancellationToken tokenl)
        {
            await Task.CompletedTask.ConfigureAwait(false);
            return new DeviceRevData();
        }

        public virtual async Task<byte[]> DeviceReadAsync<T>(T mCmd, CancellationToken token)
        {
            await Task.CompletedTask.ConfigureAwait(false);
            return null;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public virtual async Task TopicSubscribe(string topic)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public virtual async Task TopicUnsubscribe(string topic)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public virtual string GetString(byte[] data, int startIndex, int mLength)
        {
            return "";
        }

        public virtual byte GetByte(byte[] data, int startIndex)
        {
            return 0;
        }

        public virtual short GetShort(byte[] data, int startIndex)
        {
            return 0;
        }

        public virtual int GetInt(byte[] data, int startIndex)
        {
            return 0;
        }

        public virtual float GetFloat(byte[] data, int startIndex)
        {
            return 0;
        }

        public virtual async Task PublishData(string topic, string message)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public virtual async Task DeviceEstop()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public virtual async Task DeviceReset()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public virtual async Task DeviceOpenAsync(JObject mPara)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public virtual async Task DeviceCloseAsync(JObject mPara)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public virtual async Task<JObject> DeviceRunAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
            return [];
        }

        public virtual async Task<JObject> DeviceRunAsync(JObject mPara)
        {
            await Task.CompletedTask.ConfigureAwait(false);
            return [];
        }

    }
}