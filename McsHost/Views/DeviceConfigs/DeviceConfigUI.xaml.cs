using CoreServiceLib.Interfaces.Diag;

using System.Windows;

namespace CoreServiceLib.Views
{
    public partial class DeviceConfigUI : Window, IDiagBaseUIService
    {
        public DeviceConfigUI()
        {
            InitializeComponent();
        }

        public string WindowID => "DeviceConfigUI";

        public void ShowDialogUI()
        {
            ShowDialog();
        }

        public void ShowUI()
        {
            Show();
        }
    }
}