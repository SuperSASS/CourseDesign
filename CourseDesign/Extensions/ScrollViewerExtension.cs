using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseDesign.Extensions
{
    /// <summary>
    /// 用于调整ScrollViewer的功能的扩展类
    /// </summary>
    internal class ScrollViewerExtension
    {
        private ScrollViewer Scroll;

        /// <summary>
        /// 将ScrollViewer鼠标滚动事件，响应为可以水平滚动。
        /// <para>使用方法：在构造函数中写入<c>AddHandler(MouseWheelEvent, new ScrollViewerExtension().HorizontalWheelHandler(*), true);</c>，其中*为传入的参数</para>
        /// </summary>
        /// <param name="scroll">需要调整的ScrollViewer控件</param>
        /// <returns></returns>
        public Delegate HorizontalWheelHandler(ScrollViewer scroll)
        {
            Scroll = scroll;
            return new MouseWheelEventHandler(ListBox_HorizontalMouseWheel);
        }

        /// <summary>
        /// 将ListBox的滚动事件与ScrollView的滚动关联，使得鼠标放在内部元素上也能滚轮滚动。
        /// <para>参考自：https://blog.csdn.net/weixin_53370274/article/details/124538587</para>
        /// </summary>
        /// <param name="fElement">传入ScrollView的直接子元素</param>
        public static void PerfectScrolling(FrameworkElement fElement)
        {
            fElement.PreviewMouseWheel += (sender, e) =>
            {
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                fElement.RaiseEvent(eventArg);
            };
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
            if (Scroll != null)
            {
                int d = e.Delta;
                // int currentNum = 0;
                // var horOffset = (int)Math.Round(scroll.HorizontalOffset);
                if (d > 0)
                {
                    Scroll.LineLeft();
                    Scroll.LineLeft();
                    // currentNum = horOffset / (int)items.ActualWidth + 1; // 不知道原作者为什么会算这个完全没用到，估计就是想用这个来确定相对滚动距离然后滚动更丝滑吧【……
                }
                if (d < 0)
                {
                    Scroll.LineRight();
                    Scroll.LineRight();
                    // currentNum = horOffset / (int)items.ActualWidth + 1;
                }
                Scroll.ScrollToTop();
            }
        }
    }
}
