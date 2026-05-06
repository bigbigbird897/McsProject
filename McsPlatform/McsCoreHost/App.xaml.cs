using HandyControl.Themes;
using McsHost.Services;

using McsWpfCore.WpfEx;

using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace McsCoreHost
{
    public partial class App : Application
    {
        private static Mutex mutex;
        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();
        private McsCoreBuilder mMcsBuilder;


        internal void UpdateTheme(ApplicationTheme theme)
        {
            if (ThemeManager.Current.ApplicationTheme != theme)
            {
                ThemeManager.Current.ApplicationTheme = theme;
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            mutex = new Mutex(true, "McsNew");
            try
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("程序已运行！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    await Closed();
                    return;
                }
                base.OnStartup(e);
                //避免不能输入小数
                FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
                //定义未处理异常统一处理方式
                DispatcherUnhandledException += App_DispatcherUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

                //打开主窗口,启动WEB服务
                mMcsBuilder = new McsCoreBuilder();
                await mMcsBuilder.HostBuilder();
                new Bootstrapper(mMcsBuilder).Run();
                mLogger.McsLogInfo("程序启动完成");

                //启动服务
                await mMcsBuilder.HostRunAsync().ConfigureAwait(false);
                mLogger.Info("WEB程序退出");
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, mLogger);
                await Closed();
                mLogger.Error(ex);
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await Closed();
            base.OnExit(e);
        }

        private async Task Closed()
        {
            await mMcsBuilder?.HostStop();
            mutex?.Dispose();
            Shutdown();
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            if (ex != null)
            {
                mLogger.Error(ex);
                e.SetObserved();
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            if (ex != null)
            {
                mLogger.Error(ex);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            if (ex != null)
            {
                MessageBox.Show($"程序启动异常{ex.Message}", "错 误", MessageBoxButton.OK, MessageBoxImage.Error);
                mLogger.Error(ex);
            }
            e.Handled = true;
        }
    }
}