using McsApplication.CommServices;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace WebAPI.Services
{
    /// <summary>
    /// 模块加载
    /// </summary>

    [DependsOn(typeof(McsAppModule))]
    public class McsWebApiModle : AbpModule
    {
        /// <summary>
        /// 模块服务配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            return base.ConfigureServicesAsync(context);
        }

        /// <summary>
        ///  模块初始化
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            return base.OnApplicationInitializationAsync(context);
        }
    }
}