using McsCoreLib.Views;
using McsCoreLib.Views.DeviceConfigs;

using McsCoreLib.Views.SubjectConfigs;
using McsCoreLib.Views.User;

using McsHost.ViewModels.DeviceConfigs;
using McsHost.ViewModels.MaterialConfigs;
using McsHost.ViewModels.SubjectConfigs;
using McsHost.ViewModels.User;

using McsWpfCore.WpfEx;

namespace McsHost.Services
{
    internal class ViewRegist
    {
        public static void RegistViewMedels(ApplicationInitializationContext context)
        {
            //物料配置界面
            context.RegisterForNavigation<MaterialConfigUI, MaterialConfigUIViewModel>(); //物料配置

            //硬件配置界面
            context.RegisterForNavigation<DeviceConfigUI, DeviceConfigUIViewModel>();
            context.RegisterForNavigation<LineConfigUI, LineConfigUIViewModel>();

            //工艺参数配置
            context.RegisterForNavigation<RecipeConfigUI, RecipeConfigUIViewModel>();

            //用户管理
            context.RegisterForNavigation<UserConfigUI, UserConfigUIViewModel>();
            context.RegisterForNavigation<AuthorityConfigUI, AuthorityConfigUIViewModel>();
            context.RegisterForNavigation<UserLoginUI, UserLoginUIViewModel>();
        }
    }
}