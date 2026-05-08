using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Services;

using Tcp.Interfaces;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace Tcp.Services
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