using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

using CoreServiceLib.Core.IocExt;

using Host.Views;

using Prism.Container.DryIoc;
using Prism.DryIoc;

using System.Windows;

namespace Host.Services
{
    internal class Bootstrapper(McsCoreBuilder mMcsBuilder) : PrismBootstrapper
    {
        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            try
            {
                mMcsBuilder.WebServiceInit();
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            IContainerExtension container = null;
            try
            {
                var imp = new Container(CreateContainerRules()).WithDependencyInjectionAdapter(McsApp.McsRegistry);
                container = new DryIocContainerExtension(imp);
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }

            return container;
        }
    }
}