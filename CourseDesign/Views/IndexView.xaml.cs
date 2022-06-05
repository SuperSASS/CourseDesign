using CourseDesign.Extensions;
using System.Windows.Controls;


// TODO: 副官改为可切换的
namespace CourseDesign.Views
{
    /// <summary>
    /// IndexView.xaml 的交互逻辑
    /// </summary>
    public partial class IndexView : UserControl
    {
        public IndexView()
        {
            InitializeComponent();
            ScrollViewerExtension.PerfectScrolling(task_list_son); // 使ScrollViewer在内部控件上也可以滚动
        }
    }
}
