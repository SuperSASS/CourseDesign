using CourseDesign.Extensions;
using System.Windows.Controls;

namespace CourseDesign.Views
{
    /// <summary>
    /// ListView.xaml 的交互逻辑
    /// </summary>
    public partial class ListView : UserControl
    {
        public ListView()
        {
            InitializeComponent();
            AddHandler(MouseWheelEvent, new ScrollViewerExtension().HorizontalWheelHandler(tdoll_list), true); // 使task_list可以横向滚动
        }

    }
}
