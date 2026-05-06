using McsCoreLib.Interfaces.Diag;

using System.Windows;

namespace McsCoreLib.Views.User
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