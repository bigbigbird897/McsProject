using CoreServiceLib.Interfaces.Diag;

using System.Windows;

namespace CoreServiceLib.Views.User
{
    /// <summary>
    /// Interaction logic for UserLoginUI.xaml
    /// </summary>
    public partial class UserLoginUI : Window, IDiagBaseUIService
    {
        public UserLoginUI()
        {
            InitializeComponent();
        }

        public string WindowID => nameof(UserLoginUI);

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