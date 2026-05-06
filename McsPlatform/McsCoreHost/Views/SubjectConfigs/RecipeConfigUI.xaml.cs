using McsCoreLib.Interfaces.Diag;

using System.Windows;

namespace McsCoreLib.Views.SubjectConfigs
{
    /// <summary>
    /// Interaction logic for RecipeConfigUI.xaml
    /// </summary>
    public partial class RecipeConfigUI : Window, IDiagBaseUIService
    {
        public RecipeConfigUI()
        {
            InitializeComponent();
        }

        public string WindowID => nameof(RecipeConfigUI);

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