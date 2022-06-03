using CourseDesign.Command.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// TaskView.xaml 的交互逻辑
    /// </summary>
    public partial class TaskView : UserControl
    {
        public TaskView()
        {
            InitializeComponent();
            AddHandler(ListBox.MouseWheelEvent, new MouseWheelEventHandler(ListBox_HorizontalMouseWheel), true);
        }

        /// <summary>
        /// 将ScrollViewer（即该页面中的水平滚动模块task_list）的鼠标滚动事件，响应为可以水平滚动。
        /// <para>参考自：http://www.manongjc.com/detail/22-ggnbscnaxjwpqhx.html 和 https://blog.csdn.net/weixin_30680385/article/details/97466338</para>
        /// </summary>
        /// <param name="sender">这个页面类</param>
        /// <param name="e">鼠标滚轮移动事件</param>
        private void ListBox_HorizontalMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // sender代表的就是窗口，则sender.AcutalWidth与滚轮的width呈线性关系，或许可以推算滚动相对距离
            // TaskView items = (TaskView)sender;
            ScrollViewer scroll = task_list;
            if (scroll != null)
            {
                int d = e.Delta;
                // int currentNum = 0;
                // var horOffset = (int)Math.Round(scroll.HorizontalOffset);
                if (d > 0)
                {
                    scroll.LineLeft();
                    scroll.LineLeft();
                    // currentNum = horOffset / (int)items.ActualWidth + 1; // 不知道原作者为什么会算这个完全没用到，估计就是想用这个来确定相对滚动距离然后滚动更丝滑吧【……
                }
                if (d < 0)
                {
                    scroll.LineRight();
                    scroll.LineRight();
                    // currentNum = horOffset / (int)items.ActualWidth + 1;
                }
                scroll.ScrollToTop();
            }
        }
    }

    /// <summary>
    /// 用于重写SelectTemplate方法的类
    /// </summary>
    public class TasksDataTemplate : DataTemplateSelector
    {
        public DataTemplate ImageTemplate { get; set; }

        public DataTemplate TextTemplate { get; set; }

        /// <summary>
        /// 重写SelectTemplate方法，使得绑定源时，能根据不同类型的数据源，绑定不同的模板。
        /// <para>实现方法：因为以确保item属于基类TasksBase，故直接将传来的object类的item给强制类型转换为TasksBase，然后根据t.Type判断即可</para>
        /// </summary>
        /// <param name="item">前端ItemControl中传到选择器来的类</param>
        /// <param name="container">不知道啥用orz……</param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            TasksBase t = (TasksBase)item;

            switch (t.Type)
            {
                case TasksBase.typeEnum.text:
                    return TextTemplate;
                case TasksBase.typeEnum.image:
                    return ImageTemplate;
                default:
                    return null;
            }
        }
    }
}
