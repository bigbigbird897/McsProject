using CoreServiceLib.Interfaces.Hardware;
using CoreServiceLib.Models.Hardware;
using CoreServiceLib.Models.Hardware.Device;

using Microsoft.Extensions.Configuration;

using System.Collections.Concurrent;

namespace CoreServiceLib.Services
{
    public class HardwareManager(IServiceProvider service) : IHardwareManager, ISingletonDependency
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentDictionary<string, IDevice> _devices = [];

        public void LineCreate()
        {
            try
            {
                var config = service.GetService<IConfiguration>();
                var line = config["LineID"];
                var linename = config["LineName"];
                var db = McsDbTool.GetDBRepositoryRef<LineConfig>();
                var data = db.AsQueryable().Where(a => a.LineID == line).First();
                if (data == null) db.Insert(new LineConfig() { LineID = line, LineName = linename });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void StationFunctionTypeRegist()
        {
            try
            {
                var sttypes = service.GetServices<IStation>();
                var db = McsDbTool.GetDBRepositoryRef<StationFunType>();
                if (sttypes != null)
                {
                    foreach (var item in sttypes)
                    {
                        db.InsertOrUpdate(item.FunTypes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public async Task DeviceTypeRegist()
        {
            try
            {
                _logger.Info("设备驱动加载开始");
                List<DeviceTypeConfig> deviceConfigs = [];
                var devices = service.GetServices<IDevice>();

                if (devices.Any())
                {
                    foreach (var item in devices)
                    {
                        deviceConfigs.Add(new DeviceTypeConfig()
                        {
                            TypeID = item.DeviceTypeID,
                            TypeName = item.DeviceTypeName
                        });
                    }
                    var mdb = McsDbTool.GetDBRepositoryRef<DeviceTypeConfig>();
                    await mdb.InsertOrUpdateAsync(deviceConfigs).ConfigureAwait(false);
                }
                _logger.Info("设备驱动加载完成");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public async Task<bool> DeviceCreate()
        {
            bool result = true;
            try
            {
                var mdb = McsDbTool.GetDBRepositoryRef<DeviceConfig>();
                var datas = await mdb.AsQueryable().ToListAsync().ConfigureAwait(false);
                if (datas.Count != 0)
                {
                    foreach (var item in datas)
                    {
                        var mdevice = service.GetServices<IDevice>().Where(a => a.DeviceTypeID == item.DeviceTypeID).FirstOrDefault();
                        if (mdevice != null)
                        {
                            if (_devices.TryGetValue(item.DeviceId, out IDevice device))
                            {
                                await device.DeviceQuit();
                                device.Dispose();
                                _devices.Remove(item.DeviceId, out device);
                                _logger.Info($"设备{item.DeviceId}重启动开始");
                            }

                            if (_devices.TryAdd(item.DeviceId, mdevice))
                            {
                                mdevice.DeviceCreate(item);
                                _logger.Info($"设备{item.DeviceId}已启动");
                            }
                        }
                    }
                }
                else
                {
                    _logger.Info("没有设备配置信息");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                result = false;
            }
            return result;
        }

        public IDevice GetDevice(string DeviceId)
        {
            _devices.TryGetValue(DeviceId, out IDevice device);
            return device;
        }

        public async Task DeviceQuit()
        {
            foreach (var item in _devices)
            {
                if (item.Value != null)
                {
                    await item.Value.DeviceQuit();
                    item.Value.Dispose();
                    _logger.Info($"设备{item.Key}已停止并释放");
                }
            }
            _devices.Clear();
        }

        public IDevice GetDeviceType(int DeviceId)
        {
            return service.GetServices<IDevice>().Where(a => a.DeviceTypeID == DeviceId).FirstOrDefault();
        }
    }
}