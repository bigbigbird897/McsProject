using McsCoreInterface.Services;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace ServiceImplement.CommServices
{
    [DependsOn(typeof(McsCoreInterfaceModle))]
    public class McsAppModule : AbpModule
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            try
            {
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            try
            {
                InitUI(context);
                BufferAreaInit(context);


            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            try
            {


            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }


        //WPF 界面绑定注册
        private static void InitUI(ApplicationInitializationContext context)
        {
            //界面注册
        }

        private static void BufferAreaInit(ApplicationInitializationContext context)
        {
        }
    }
}