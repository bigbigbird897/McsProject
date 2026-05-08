using CoreServiceLib.Interfaces.Diag;

using System.Windows;

namespace UIWpf.Views
{
    /// <summary>
    /// Interaction logic for MqttConfig.xaml
    /// </summary>
    public partial class MqttConfig : Window, IDiagBaseUIService
    {
        public MqttConfig()
        {
            InitializeComponent();
        }

        public string WindowID => "HitbotMqttConfigUI";

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