using CourseDesign.Command.Classes;
using CourseDesign.Extensions;
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
            AddHandler(MouseWheelEvent, new ScrollViewerExtension().HorizontalWheelHandler(task_list), true); // 使task_list可以横向滚动
        }
    }

    /// <summary>
    /// 用于重写SelectTemplate方法的类，使得可以根据不同的计划类型绑定不同的模板
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
