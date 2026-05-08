using Host;
using CoreWebAPI.CoreMessages;

using CoreServiceLib.Core.McsEvent;

using CoreWpfLib.WpfEx;

using Microsoft.Extensions.Configuration;

using Prism.Events;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Host.Views
{
    public partial class MainWindow
    {
        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();
        private readonly bool isDisable = false;
        private readonly bool IsOpenWeb = false; //是否打开网页
        private readonly string mBrowserPath = string.Empty; //浏览器路径
        private readonly string mUrl = string.Empty; //网页地址
        private readonly bool Isfullscreen;
        private bool IsOpened = false;

        public MainWindow(IConfiguration configuration)
        {
            try
            {
                InitializeComponent();
                ((App)Application.Current).UpdateTheme(HandyControl.Themes.ApplicationTheme.Dark);

                McsEventMessage.Instance.Subscribe<ChangeWindow>(DoChange, ThreadOption.UIThread);
                isDisable = configuration.GetValue<bool>("WindowHide");
                IsOpenWeb = configuration.GetValue<bool>("IsOpenWeb");
                mBrowserPath = configuration.GetValue<string>("Browserpath");
                mUrl = configuration.GetValue<string>("BaseUrl");
                Isfullscreen = configuration.GetValue<bool>("Isfullscreen");
                WindowState = isDisable ? WindowState.Minimized : WindowState.Maximized;
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, mLogger);
            }
        }

        private void DoChange(ChangeWindow window)
        {
            switch (window.ChageM)
            {
                case ChangeType.Min:
                    WindowState = WindowState.Minimized;
                    break;

                case ChangeType.Max:
                    WindowState = WindowState.Maximized;
                    break;

                case ChangeType.Close:
                    WindowMain.Close();
                    break;

                default:
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isDisable) Hide();
                else WindowState = WindowState.Maximized;
                OpenWeb();
            }
            catch (Exception ex)
            {
                McsDiag.SendErr(ex, mLogger);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                //关闭浏览器
                if (IsOpened)
                {
                    foreach (var item in Process.GetProcessesByName("msedge"))
                    {
                        item.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }
        }

        private void OpenWeb()
        {
            try
            {
                if (IsOpenWeb && !string.IsNullOrEmpty(mBrowserPath) && !string.IsNullOrEmpty(mUrl))
                {
                    ProcessStartInfo psi = new(mBrowserPath)
                    {
                        Arguments = Isfullscreen ? $"--new-window {mUrl} --kiosk --edge - kiosk - type = fullscreen" : $"--new-window {mUrl}",
                        CreateNoWindow = true
                    };
                    Process.Start(psi);
                    IsOpened = true;
                }
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }
        }
    }
}