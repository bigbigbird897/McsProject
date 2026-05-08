using CoreServiceLib.Interfaces.Diag;

using System.Windows;

namespace CoreServiceLib.Views.DeviceConfigs
{
    /// <summary>
    /// Interaction logic for LineConfigUI.xaml
    /// </summary>
    public partial class LineConfigUI : Window, IDiagBaseUIService
    {
        public LineConfigUI()
        {
            InitializeComponent();
        }

        public string WindowID => nameof(LineConfigUI);

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