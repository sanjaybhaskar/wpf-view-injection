using ModuleA.RibbonTabs;
using PrismDemo.Core;
using System.Windows.Controls;

namespace ModuleA.Views
{
    /// <summary>
    /// Interaction logic for ViewB
    /// </summary>
    [DependentView(typeof(ViewBTab), "RibbonTabRegion")]
    [DependentView(typeof(ViewC), "SubRegion")]
    public partial class ViewB : UserControl, ISupportDataContext
    {
        public ViewB()
        {
            InitializeComponent();
        }
    }
}
