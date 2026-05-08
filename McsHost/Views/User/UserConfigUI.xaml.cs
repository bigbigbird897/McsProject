using CoreServiceLib.Interfaces.Diag;

using System.Windows;

namespace CoreServiceLib.Views.User
{
    /// <summary>
    /// Interaction logic for UserConfigUI.xaml
    /// </summary>
    public partial class UserConfigUI : Window, IDiagBaseUIService
    {
        public UserConfigUI()
        {
            InitializeComponent();
        }

        public string WindowID => nameof(UserConfigUI);

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