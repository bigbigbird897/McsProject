using McsCoreLib.Services;

namespace HitbotMqtt.Services
{
    [DependsOn(typeof(McsCoreModule))]
    public class HitbotMqttModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }

        public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            return base.OnApplicationInitializationAsync(context);
        }
    }
}