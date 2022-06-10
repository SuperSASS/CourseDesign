using CourseDesign.ViewModels.Dialogs.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CourseDesign.Services.Dialog
{
    // 
    // 注意这只是个主机（服务），是在
    /// <summary>
    /// 自定义的对话主机服务（是扩展的Dialog服务的实现）
    /// <para>注意这只是个主机（服务），提供<c>ShowDialog</c>方法展示对话框，但各种对话框的页面V和逻辑VM还要自己实现</para>
    /// </summary>
    public class DialogHostService : DialogService, IDialogHostService
    {
        private readonly IContainerExtension containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        // 对这个服务的唯一方法(x)ShowDialog的实现
        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "RootDialog")
        {
            if (parameters == null)
                parameters = new DialogParameters();
            // 从容器中取出弹出窗口的实例（窗口上下文，包含各种属性等）
            var context = containerExtension.Resolve<object>(name);
            // 验证实例的有效性
            if (context is not FrameworkElement dialogContext)
                throw new NullReferenceException("内部错误(Dialog) - 不是有效的FrameworkElement");
            // 创建V与VM之间的上下文联系
            if (dialogContext is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);
            if (dialogContext.DataContext is not IDialogHostAware viewModel)
                throw new NullReferenceException("内部错误(Dialog) - 上下文联系建立失败，Dialog的VM必须继承于\"IDialogAware\"接口");
            /* 以下为调用MaterialDesign里的Dialog */
            viewModel.DialogHostName = dialogHostName;
            DialogOpenedEventHandler eventHandler = (s, e) => // 这里新建了一个Handler，就是用来处理MaterialDesign里的Dialog的
            {
                if (viewModel is IDialogHostAware aware)
                    aware.OnDialogOpen(parameters);
                e.Session.UpdateContent(context);
            };
            var WATCH = (IDialogResult)await DialogHost.Show(dialogContext, viewModel.DialogHostName, eventHandler);
            return WATCH;
            //return (IDialogResult)await DialogHost.Show(dialogContext, viewModel.DialogHostName, eventHandler);
        }
    }
}
