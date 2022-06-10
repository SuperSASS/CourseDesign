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
    public class DialogHostService : DialogService, IDialogHostService
    {
        private readonly IContainerExtension containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "root")
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
                throw new Exception("内部错误");
            viewModel.DialogHostName = dialogHostName;

            DialogOpenedEventHandler eventHandler = (s, e) =>
            {
                if (viewModel is IDialogHostAware aware)
                    aware.OnDialogOpen(parameters);
                e.Session.UpdateContent(content);
            };

            return (IDialogResult)await DialogHost.Show(dialogContent, viewModel.DialogHostName, eventHandler);
        }
    }
}
