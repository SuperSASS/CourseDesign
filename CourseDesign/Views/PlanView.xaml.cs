using CourseDesign.Command.Classes;
using CourseDesign.Extensions;
using System.Windows;
using System.Windows.Controls;
using CourseDesign.Context;

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

    /// <summary>
    /// 用于重写SelectTemplate方法的类，使得可以根据不同的计划类型绑定不同的模板
    /// </summary>
    public class PlansDataTemplate : DataTemplateSelector
    {
        public DataTemplate ImageTemplate { get; set; }
        public DataTemplate TextTemplate { get; set; }

        /// <summary>
        /// 重写SelectTemplate方法，使得绑定源时，能根据不同类型的数据源，绑定不同的模板。
        /// <para>实现方法：因为以确保item属于基类PlansBase，故直接将传来的object类的item给强制类型转换为PlansBase，然后根据t.Type判断即可</para>
        /// </summary>
        /// <param name="item">前端ItemControl中传到选择器来的类</param>
        /// <param name="container">不知道啥用orz……</param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            PlanBase t = (PlanBase)item;

            switch (t.Type)
            {
                case PlanBase.TypeEnum.text:
                    return TextTemplate;
                case PlanBase.TypeEnum.image:
                    return ImageTemplate;
                default:
                    return null;
            }
        }
    }
}
