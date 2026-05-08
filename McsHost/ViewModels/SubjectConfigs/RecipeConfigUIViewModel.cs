using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Interfaces.Workflow;
using CoreServiceLib.Models.Workflow;
using CoreServiceLib.Paras;
using CoreServiceLib.Tools;
using CoreWpfLib.WpfEx;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Host.ViewModels.SubjectConfigs
{
    public class RecipeConfigUIViewModel : BindableBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public List<StepType> StepTypes { get; set; } = [];
        public ObservableCollection<WorkflowConfig> RecipeConfigs { get; set; } = [];

        private WorkflowConfig mSelectRecipe;

        public WorkflowConfig SelectRecipe
        {
            get { return mSelectRecipe; }
            set { SetProperty(ref mSelectRecipe, value); }
        }

        private StepInfo mSelectStep;

        public StepInfo SelectStep
        {
            get { return mSelectStep; }
            set { SetProperty(ref mSelectStep, value); }
        }

        public DelegateCommand AddRecipeItem { get; }
        public DelegateCommand RemoveRecipeItem { get; }
        public DelegateCommand AddStepItem { get; }
        public DelegateCommand RemoveStepItem { get; }
        public DelegateCommand SetStepPara { get; }
        public DelegateCommand<Window> SaveData { get; }
        public DelegateCommand<Window> Quit { get; }

        public RecipeConfigUIViewModel()
        {
            try
            {

                //加载步序类型
                var msttdb = McsDbTool.GetDBRepositoryRef<StepType>();
                var msttdata = msttdb.AsQueryable().ToList();
                if (msttdata != null) StepTypes.AddRange(msttdata);

                //加载配方信息
                var mrepiceDb = McsDbTool.GetDBRepositoryRef<WorkflowConfig>();
                var mrepiceData = mrepiceDb.AsQueryable().ToList();
                if (mrepiceData != null) RecipeConfigs.AddRange(mrepiceData);

                Quit = new DelegateCommand<Window>(DoQuit);
                SaveData = new DelegateCommand<Window>(DoSaveData);
                SetStepPara = new DelegateCommand(OpenParaSetUI);
                RemoveStepItem = new DelegateCommand(DoStepRemove);
                AddStepItem = new DelegateCommand(DoAddStepItem);
                RemoveRecipeItem = new DelegateCommand(DoDelRecipeItem);
                AddRecipeItem = new DelegateCommand(DoAddRecipeItem);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoAddRecipeItem()
        {
            try
            {
                RecipeConfigs.Add(new WorkflowConfig()
                {
                    WorkflowID = $"Recipe{DateTime.Now:yyyyMMddHmmssfff}",
                    Steps = []
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoDelRecipeItem()
        {
            try
            {
                if (SelectRecipe != null)
                {
                    RecipeConfigs.Remove(SelectRecipe);
                    SelectRecipe = null;
                    SelectStep = null;
                    McsDiag.OkMessage("数据删除完成!");
                }
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }

        private void DoAddStepItem()
        {
            try
            {
                var couter = SelectRecipe.Steps.Count;
                ++couter;
                SelectRecipe?.Steps.Add(new StepInfo()
                {
                    IsDisable = false,
                    StepID = couter
                }
                );
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }

        private void DoStepRemove()
        {
            try
            {
                if (SelectStep != null && McsDiag.QueMessage("删除当前选择项?"))
                {
                    SelectRecipe.Steps.Remove(SelectStep);
                    SelectStep = null;
                    McsDiag.OkMessage("数据删除完成!");
                }
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }

        private void OpenParaSetUI()
        {
            try
            {
                if (SelectStep != null)
                {
                    var step = McsApp.McsServiceProvider.Resolve<List<IStep>>().SingleOrDefault(a => a.StepTypeID == SelectStep.StepTypeID);
                    step?.OpenConfingUI(SelectStep.StepPara);
                }
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
            }
        }

        private void DoSaveData(Window window)
        {
            //配方编号唯一性验证
            if (RecipeConfigs.Any(a => string.IsNullOrEmpty(a.WorkflowID)))
            {
                McsDiag.WarningMessage("配方编号不允许为空");
                return;
            }

            foreach (var item in RecipeConfigs)
            {
                if (RecipeConfigs.Where(a => a.WorkflowID.Equals(item.WorkflowID, StringComparison.CurrentCultureIgnoreCase)).Count() > 1)
                {
                    McsDiag.WarningMessage("配方编号不唯一");
                    return;
                }
            }

            foreach (var item in RecipeConfigs)
            {
                foreach (var item1 in item.Steps)
                {
                    if (item.Steps.Where(a => a.StepTypeID == item1.StepTypeID).Count() > 1)
                    {
                        McsDiag.WarningMessage("实验步骤类型不允许重复");
                        return;
                    }
                }
            }

            var mrepiceDb = McsDbTool.GetDBRepositoryRef<WorkflowConfig>();
            try
            {
                mrepiceDb.AsTenant().BeginTran();
                mrepiceDb.Context.Deleteable<WorkflowConfig>().Where(a => 1 == 1).ExecuteCommand();
                mrepiceDb.Context.Insertable(RecipeConfigs.ToList()).ExecuteCommand();
                mrepiceDb.AsTenant().CommitTran();
                McsDiag.OkMessage("数据保存完成");
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, _logger);
                mrepiceDb?.AsTenant().RollbackTran();
            }
        }

        private void DoQuit(Window window)
        {
            window.Close();
        }
    }
}