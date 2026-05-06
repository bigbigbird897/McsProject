using McsCoreLib.Core.IocExt;

using McsWpfCore.WpfEx;

using Volo.Abp;

namespace McsWpfCore.WpfEx
{
    public static class McsAppInitEx
    {
        public static ApplicationInitializationContext RegisterForNavigation<TView>(this ApplicationInitializationContext context, string mVIewName = null)
        {
            var con = ContainerLocator.Current;
            con.RegisterForNavigation<TView>(mVIewName);
            return context;
        }

        public static ApplicationInitializationContext RegisterForNavigation<TView, TViewModel>(this ApplicationInitializationContext context, string mVIewName = null)
        {
            var con = ContainerLocator.Current;
            con.RegisterForNavigation<TView, TViewModel>(mVIewName);
            return context;
        }

        public static ApplicationInitializationContext RegisterForNavigation(this ApplicationInitializationContext context, Type type, string mVIewName = null)
        {
            var con = ContainerLocator.Current;
            con.RegisterForNavigation(type, mVIewName);
            return context;
        }

        public static IContainerProvider McsProvider(this ApplicationInitializationContext _)
        {
            return McsApp.McsServiceProvider;
        }

        public static IContainerProvider McsProvider(this ApplicationShutdownContext _)
        {
            return McsApp.McsServiceProvider;
        }
    }
}