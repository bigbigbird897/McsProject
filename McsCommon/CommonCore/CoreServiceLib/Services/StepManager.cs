using CoreServiceLib.Interfaces.Workflow;
using CoreServiceLib.Models.Workflow;

namespace CoreServiceLib.Services
{
    public class StepManager(IServiceProvider service) : IStepManager, ISingletonDependency
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public void StepTypeRegist()
        {
            try
            {
                _logger.Info("设备命令注册开始");
                var db = McsDbTool.GetDBRepositoryRef<StepType>();
                var msc = service.GetServices<IStep>();
                if (db == null)
                {
                    _logger.Error(new Exception("命令加载失败，数据库连接错误"));
                    return;
                }
                List<StepType> types = [];
                foreach (var item in msc)
                {
                    types.Add(new StepType()
                    {
                        StepTypeID = item.StepTypeID,
                        StepTypeName = item.StepTypeName
                    });
                }

                db.InsertOrUpdate(types);
                _logger.Info("设备命令注册完成");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
