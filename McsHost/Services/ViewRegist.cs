using CoreServiceLib.Views;
using CoreServiceLib.Views.DeviceConfigs;

using CoreServiceLib.Views.SubjectConfigs;
using CoreServiceLib.Views.User;

using Host.ViewModels.DeviceConfigs;
using Host.ViewModels.MaterialConfigs;
using Host.ViewModels.SubjectConfigs;
using Host.ViewModels.User;

using CoreWpfLib.WpfEx;

namespace Host.Services
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