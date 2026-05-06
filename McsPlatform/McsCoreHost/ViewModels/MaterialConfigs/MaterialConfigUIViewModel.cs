using McsCoreLib.Interfaces.Material;
using McsCoreLib.Models.Material;

using McsWpfCore.WpfEx;

using System.Collections.ObjectModel;
using System.Windows;

namespace McsHost.ViewModels.MaterialConfigs
{
    public class MaterialConfigUIViewModel : BindableBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMaterialManager _materialManager;
        

        //
        public ObservableCollection<MaterialTypeConfig> MaterialConfigs { get; private set; } = [];

        public ObservableCollection<MaterialConfig> Materials { get; set; } = [];

        //
        private MaterialConfig mSelectConfig;

        public MaterialConfig SelectConfig
        {
            get { return mSelectConfig; }
            set { SetProperty(ref mSelectConfig, value); }
        }

        public DelegateCommand<Window> DataSave { get; }
        public DelegateCommand<Window> SaveCancel { get; }
        public DelegateCommand OpenParaUI { get; }

        public DelegateCommand AddItem { get; }
        public DelegateCommand RemoveItem { get; }

        public MaterialConfigUIViewModel(IMaterialManager manager)
        {
            _materialManager = manager;
            
            DataSave = new DelegateCommand<Window>(DoSave);
            SaveCancel = new DelegateCommand<Window>(DoSaveCancel);
            OpenParaUI = new DelegateCommand(DoOpenParaUI);
            AddItem = new DelegateCommand(DoAdd);
            RemoveItem = new DelegateCommand(DoRemove);

            try
            {
                var db = McsDbTool.GetDBRepositoryRef < MaterialTypeConfig>();
                var data = db.AsQueryable().Where(a => a.MaterialTypeID != 3 && a.MaterialTypeID != 4).ToList();
                if (data.Count > 0)
                {
                    MaterialConfigs.Clear();
                    MaterialConfigs.AddRange(data);
                }

                var mDb = McsDbTool.GetDBRepositoryRef < MaterialConfig>();
                var mdatas = mDb.AsQueryable().ToList();
                if (mdatas.Count > 0)
                {
                    Materials.Clear();
                    Materials.AddRange(mdatas);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoRemove()
        {
            try
            {
                if (SelectConfig != null && McsDiag.QueMessage("删除当前项?"))
                {
                    var db = McsDbTool.GetDBRepositoryRef < MaterialConfig>();
                    var data = db.AsQueryable().Where(a => a.MaterialID == SelectConfig.MaterialID).First();
                    if (data != null) db.Delete(SelectConfig);
                    Materials.Remove(SelectConfig);
                    SelectConfig = null;
                }
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }

        private void DoAdd()
        {
            try
            {
                Materials.Add(new MaterialConfig()
                {
                    MaterialID = $"N{DateTime.Now:yyyyMMddHHmmssfff}",
                });
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }

        private void DoOpenParaUI()
        {
            try
            {
                if (SelectConfig != null)
                {
                    if (SelectConfig.MaterialTypeID <= 0)
                    {
                        McsDiag.WarningMessage("物料类型未选择！");
                        return;
                    }
                    var (material, config) = _materialManager.GetMaterial(SelectConfig.MaterialTypeID);
                    SelectConfig.MaterialTypePara ??= default;
                    SelectConfig.MaterialTypePara = material.MaterialParaConfigUI(SelectConfig.MaterialTypePara);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoSaveCancel(Window window)
        {
            window.Close();
        }

        private void DoSave(Window window)
        {
            try
            {
                var db = McsDbTool.GetDBRepositoryRef < MaterialConfig>();

                // 物料编号为空检查
                if (Materials.Where(a => string.IsNullOrEmpty(a.MaterialID)).Any())
                {
                    McsDiag.WarningMessage($"物料编码为空，不允许保存");
                    return;
                }

                //物料条码唯一性验证
                foreach (var item in Materials)
                {
                    var x = Materials.Where(a => a.MaterialID == item.MaterialID).ToList();
                    if (x.Count > 1)
                    {
                        McsDiag.WarningMessage($"物料编码重复，不允许保存");
                        return;
                    }
                }

                //删除记录
                db.Context.Deleteable<MaterialConfig>().Where(a => 1 == 1).ExecuteCommand();

                foreach (var item in Materials)
                {
                    db.InsertOrUpdate(item);
                }
                McsDiag.OkMessage("数据保存完成");
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }
    }
}