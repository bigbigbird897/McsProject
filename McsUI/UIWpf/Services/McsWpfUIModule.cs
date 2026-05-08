using ServiceImplement.CommServices;
using NLog;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace UIWpf.Services
{
    [DependsOn(typeof(McsAppModule))]
    public class McsWpfUIModule : AbpModule
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


        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            return base.ConfigureServicesAsync(context);
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            try
            {

            }
            catch (Exception ex)
            {

                _logger.Error(ex);
            }
        }
    }
}
