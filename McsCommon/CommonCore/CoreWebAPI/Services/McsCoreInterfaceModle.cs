using HitbotMqtt.Services;

using CoreServiceLib.Services;

using Tcp.Services;

using Volo.Abp.Modularity;

namespace McsCoreInterface.Services
{
    /// <summary>
    /// 注册模块
    /// </summary>
    [DependsOn(typeof(McsCoreModule),
        typeof(HitbotMqttModule),
        typeof(McsTcpModule))]
    public class McsCoreInterfaceModle : AbpModule
    {
    }
}