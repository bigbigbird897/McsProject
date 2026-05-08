using CoreServiceLib.Core.Attributes;

using System.Windows.Controls;

namespace UIWpf.Views
{
    /// <summary>
    /// Interaction logic for AppStatus
    /// </summary>
    [McsRegion("StatusBarRegion")]
    public partial class AppStatus : UserControl
    {
        public AppStatus()
        {
            InitializeComponent();
        }
    }
}