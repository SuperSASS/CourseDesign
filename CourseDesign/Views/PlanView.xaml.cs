using CourseDesign.Extensions;
using System.Windows;
using System.Windows.Controls;
using CourseDesign.ViewModels;
using CourseDesign.Common.Classes.Bases;

namespace CourseDesign.Views
{
    /// <summary>
    /// PlanView.xaml 的交互逻辑
    /// </summary>
    public partial class PlanView : UserControl
    {
        public PlanView()
        {
            InitializeComponent();
            AddHandler(MouseWheelEvent, new ScrollViewerExtension().HorizontalWheelHandler(task_list), true); // 使task_list可以横向滚动
        }
    }
}
