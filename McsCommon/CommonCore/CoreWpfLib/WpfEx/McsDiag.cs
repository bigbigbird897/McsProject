using NLog;

using System.Windows;

namespace CoreWpfLib.WpfEx
{
    public class McsDiag
    {
        public static void SendErr(Exception ex, ILogger mLog)
        {
            mLog.McsLogErr(ex);
            MessageBox.Show($"{ex.Message}", "异常", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
        }

        public static void OkMessage(string message)
        {
            MessageBox.Show($"{message}", "提示", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
        }

        public static bool QueMessage(string message)
        {
            return MessageBox.Show($"{message}", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes;
        }

        public static void WarningMessage(string message)
        {
            MessageBox.Show($"{message}", "提示", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}