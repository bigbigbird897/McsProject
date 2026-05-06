using McsCoreLib.Models.Hardware;

using McsWpfCore.WpfEx;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace McsHost.ViewModels.DeviceConfigs
{
    public class LineConfigUIViewModel : BindableBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly string mLineID;
        public ObservableCollection<StationConfig> Stations { get; set; } = [];
        public List<StationFunType> FunTypes { get; set; } = [];

        private StationConfig mSelectSt;

        public StationConfig SelectSt
        {
            get { return mSelectSt; }
            set { SetProperty(ref mSelectSt, value); }
        }

        public DelegateCommand AddItem { get; }
        public DelegateCommand RemoveItem { get; }

        public DelegateCommand<Window> DataSave { get; }
        public DelegateCommand<Window> Quit { get; }

        public LineConfigUIViewModel(IConfiguration config)
        {
            Quit = new DelegateCommand<Window>(DoQuit);
            DataSave = new DelegateCommand<Window>(DaSave);
            RemoveItem = new DelegateCommand(DoRemove);
            AddItem = new DelegateCommand(DoAdd);
            try
            {
                mLineID = config["LineID"];
                var mlineDb = McsDbTool.GetDBRepositoryRef < StationConfig>();
                var datas = mlineDb.AsQueryable().ToList();
                if (datas.Count > 0)
                {
                    Stations.AddRange(datas);
                }

                var funDb = McsDbTool.GetDBRepositoryRef < StationFunType>();
                var fundata = funDb.AsQueryable().ToList();
                if (fundata.Count > 0)
                {
                    FunTypes.AddRange(fundata);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoAdd()
        {
            Stations.Add(new StationConfig()
            {
                LineID = mLineID
            });
        }

        private void DoRemove()
        {
            try
            {
                if (SelectSt != null && McsDiag.QueMessage("删除当前选择项?"))
                {
                    if (!string.IsNullOrEmpty(SelectSt.StationID))
                    {
                        var mlineDb = McsDbTool.GetDBRepositoryRef < StationConfig>();
                        mlineDb.Delete(SelectSt);
                    }
                    Stations.Remove(SelectSt);
                    SelectSt = null;
                }
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }

        private void DaSave(Window window)
        {
            try
            {
                //工位编号为空检查
                if (Stations.Where(a => string.IsNullOrEmpty(a.StationID)).Any())
                {
                    McsDiag.WarningMessage("工位编号不允许为空");
                    return;
                }
                //唯一性验证
                foreach (var stat in Stations)
                {
                    if (Stations.Where(a => a.StationID == stat.StationID).Count() > 1)
                    {
                        McsDiag.WarningMessage("工位编号不允许重复");
                        return;
                    }
                }

                var mlineDb = McsDbTool.GetDBRepositoryRef < StationConfig>();
                //删除所有记录，避免工位编号修改后多项问题
                mlineDb.Context.Deleteable<StationConfig>().Where(a => 1 == 1).ExecuteCommand();
                mlineDb.Context.Insertable(Stations.ToList()).ExecuteCommand();
                McsDiag.OkMessage("数据保存成功");
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }

        private void DoQuit(Window window)
        {
            window.Close();
        }
    }
}