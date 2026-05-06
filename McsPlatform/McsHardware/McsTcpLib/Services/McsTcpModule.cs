using McsCoreLib.Core.IocExt;
using McsCoreLib.Services;

using McsTcpLib.Interfaces;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace McsTcpLib.Services
{
    [DependsOn(typeof(McsCoreModule))]
    public class McsTcpModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var tcpM = McsApp.McsServiceProvider.Resolve<IMcsTcpManage>();
            tcpM.TcpServerRun();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var tcpM = McsApp.McsServiceProvider.Resolve<IMcsTcpManage>();
            tcpM.TcpServerStop();
        }
    }
}