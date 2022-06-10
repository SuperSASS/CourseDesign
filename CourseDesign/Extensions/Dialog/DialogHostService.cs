using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseDesign.Extensions.Dialog
{
    // 自定义的对话主机服务（是扩展的Dialog服务的实现）
    // 注意这只是个主机（服务），是在
    public class DialogHostService : DialogService, IDialogHostService
    {
        private readonly IContainerExtension containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        // 就这里，这个主机就负责展示Dialog
        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "RootDialog")
        {
            if (parameters == null)
                parameters = new DialogParameters();
            // 从容器中取出弹出窗口的实例
            var content = containerExtension.Resolve<object>();
            // 验证实例的有效性
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("内部错误 - 不是有效的FrameworkElement");
            // 创建V与VM之间的上下文联系
            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);
            if (!(dialogContent.DataContext is IDialogHostAware viewModel))
                throw new NullReferenceException("内部错误 - 上下文联系建立失败，Dialog的VM必须继承于\"IDialogAware\"接口");

            viewModel.DialogHostName = dialogHostName;
            // 执行MaterialDesign里的Dialog
            DialogOpenedEventHandler eventHandler = (s, e) =>
            {
                if (viewModel is IDialogHostAware aware)
                    aware.OnDialogOpen(parameters);
                e.Session.UpdateContent(content);
            };

            // 这里应该就是调用Prism的Dialog服务，但给的Handler是MaterialDesign的，因此建立了联系
            return (IDialogResult)await DialogHost.Show(dialogContent, viewModel.DialogHostName, eventHandler);
        }
    }
}
