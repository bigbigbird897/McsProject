using CoreServiceLib.Interfaces.Diag;

using System.Windows;

namespace CoreServiceLib.Views.User
{
    /// <summary>
    /// Interaction logic for AuthorityConfigUI.xaml
    /// </summary>
    public partial class AuthorityConfigUI : Window, IDiagBaseUIService
    {
        public AuthorityConfigUI()
        {
            InitializeComponent();
        }

        public string WindowID => nameof(AuthorityConfigUI);

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