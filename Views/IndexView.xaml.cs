using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            UseTheScrollViewerScrolling(fElement: task_list_son);
        }

        /// <summary>
        /// 将ListBox的滚动事件与ScrollView的滚动关联，使得鼠标放在内部元素上也能滚轮滚动。
        /// <para>参考自：https://blog.csdn.net/weixin_53370274/article/details/124538587</para>
        /// </summary>
        /// <param name="fElement">传入ScrollView的直接子元素</param>
        private static void UseTheScrollViewerScrolling(FrameworkElement fElement)
        {
            fElement.PreviewMouseWheel += (sender, e) =>
            {
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                fElement.RaiseEvent(eventArg);
            };
        }
    }
}
