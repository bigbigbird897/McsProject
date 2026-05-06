using McsCoreLib.Interfaces.Diag;

using System.Windows;

namespace McsCoreLib.Views
{
    /// <summary>
    /// Interaction logic for MaterialConfigUI.xaml
    /// </summary>
    public partial class MaterialConfigUI : Window, IDiagBaseUIService
    {
        public MaterialConfigUI()
        {
            InitializeComponent();
        }

        public string WindowID => "MaterialConfigUI";

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