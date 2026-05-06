using McsCoreLib.Interfaces.Hardware;
using McsCoreLib.Models.Hardware;
using McsCoreLib.Models.Hardware.Device;

using McsWpfCore.WpfEx;

using Microsoft.Extensions.Configuration;



using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace McsHost.ViewModels.DeviceConfigs
{
    public class DeviceConfigUIViewModel : BindableBase
    {
        private readonly IHardwareManager _deviceManager;
        private readonly string mLineID;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        public ObservableCollection<DeviceConfig> DeviceConfigs { get; set; } = [];
        public ObservableCollection<LineConfig> LineConfigs { get; set; } = [];
        public ObservableCollection<StationConfig> Stations { get; set; } = [];
        public ObservableCollection<DeviceTypeConfig> DeviceTypes { get; set; } = [];

        private DeviceConfig mSelectItem;

        public DeviceConfig SelectConfig
        {
            get { return mSelectItem; }
            set
            {
                SetProperty(ref mSelectItem, value);
            }
        }

        public DelegateCommand AddItem { get; private set; }
        public DelegateCommand DeleteItem { get; private set; }
        public DelegateCommand OpenParaConfigUI { get; private set; }

        public DelegateCommand DataSave { get; private set; }
        public DelegateCommand<Window> SaveCancel { get; private set; }

        public DeviceConfigUIViewModel(IHardwareManager manager, IConfiguration config)
        {
            _deviceManager = manager;
            AddItem = new DelegateCommand(DoAdd);
            DeleteItem = new DelegateCommand(DoDelete);
            OpenParaConfigUI = new DelegateCommand(DoOpenParaConfigUI);
            DataSave = new DelegateCommand(DoSave);
            SaveCancel = new DelegateCommand<Window>(DoCancel);

            try
            {
                mLineID = config["LineID"];
                var desc = McsDbTool.GetDBRepositoryRef < DeviceConfig>();
                var descd = desc.AsQueryable().ToList();
                if (descd.Count > 0)
                {
                    DeviceConfigs.Clear();
                    DeviceConfigs.AddRange(descd);
                }

                var lindb = McsDbTool.GetDBRepositoryRef < LineConfig>();
                var linec = lindb.AsQueryable().ToList();
                if (linec.Count > 0)
                {
                    LineConfigs.Clear();
                    LineConfigs.AddRange(linec);
                }

                var st = McsDbTool.GetDBRepositoryRef < StationConfig>();
                var stdatas = st.AsQueryable().ToList();
                if (stdatas.Count != 0)
                {
                    Stations.Clear();
                    Stations.AddRange(stdatas);
                }

                var devicedb = McsDbTool.GetDBRepositoryRef < DeviceTypeConfig>();
                var devs = devicedb.AsQueryable().ToList();
                if (devs.Count != 0)
                {
                    DeviceTypes.Clear();
                    DeviceTypes.AddRange(devs);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoAdd()
        {
            try
            {
                DeviceConfigs.Add(new DeviceConfig() { LineID = mLineID });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoDelete()
        {
            if (SelectConfig != null && McsDiag.QueMessage("删除当前配置选中配置"))
            {
                try
                {
                    var x = McsDbTool.GetDBRepositoryRef < DeviceConfig>();
                    var y = x.AsQueryable().Where(a => a.DeviceId == SelectConfig.DeviceId).First();
                    if (y != null)
                    {
                        x.Delete(SelectConfig);
                    }
                    DeviceConfigs.Remove(SelectConfig);
                    SelectConfig = null;
                }
                catch (Exception ex)
                {
                    McsDiag.SendErr(ex, _logger);
                }
            }
            ;
        }

        private void DoOpenParaConfigUI()
        {
            try
            {
                if (SelectConfig != null)
                {
                    if (SelectConfig.DeviceTypeID < 1)
                    {
                        McsDiag.WarningMessage("设备类型未选择！");
                        return;
                    }
                    var dv = _deviceManager.GetDeviceType(SelectConfig.DeviceTypeID);
                    SelectConfig.DevicePara = dv?.DeviceConfigUI(SelectConfig.DevicePara);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoSave()
        {
            try
            {
                //设备编号为空判断
                if (DeviceConfigs.Where(a => string.IsNullOrEmpty(a.DeviceId)).Any())
                {
                    McsDiag.WarningMessage("设备编号不允许为空");
                    return;
                }

                foreach (var item in DeviceConfigs)
                {
                    if (DeviceConfigs.Where(a => a.DeviceId == item.DeviceId).Count() > 1)
                    {
                        McsDiag.WarningMessage("设备编号不唯一");
                        return;
                    }
                }

                var x = McsDbTool.GetDBRepositoryRef<DeviceConfig > ();
                // 删除记录
                x.Context.Deleteable<DeviceConfig>().Where(a => 1 == 1).ExecuteCommand();

                foreach (var item in DeviceConfigs)
                {
                    x.InsertOrUpdate(item);
                }
                if (McsDiag.QueMessage("数据保存完成, 是否重启动硬件?"))
                {
                    _deviceManager.DeviceQuit();
                    Thread.Sleep(2000);
                    _deviceManager.DeviceCreate();
                    McsDiag.OkMessage("硬件重启完成");
                }
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }

        private void DoCancel(Window para)
        {
            para.Close();
        }
    }
}