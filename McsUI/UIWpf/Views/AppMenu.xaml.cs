using CoreServiceLib.Core.Attributes;

using System.Windows.Controls;

namespace UIWpf.Views
{
    /// <summary>
    /// Interaction logic for AppMenu
    /// </summary>
    ///
    [McsRegion("MenuRegion")]
    public partial class AppMenu : UserControl
    {
        public AppMenu()
        {
            InitializeComponent();
        }
    }
}