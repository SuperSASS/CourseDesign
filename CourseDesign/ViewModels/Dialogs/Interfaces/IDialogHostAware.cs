using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.ViewModels.Dialogs.Interfaces
{
    /// <summary>
    /// 这是用给各种Dialog的VM继承的，如AddTextPlanViewModel
    /// </summary>
    /// 包含对话框Dialog的基本命令，如确认按钮"SaveCommand"、当打开时执行命令"OnDialogOpen"
    public interface IDialogHostAware // 此处并不需要继承IDialogAware，因为并不需要那么多方法，只用我们所需要的方法即可。
    {
        /// <summary>
        /// 所属DialogHost的主机（服务）名称，对应在View中显示到哪（默认为RootDialog）
        /// </summary>
        string DialogHostName { get; set; }

        /// <summary>
        /// 当打开对话时，会接受参数并执行这里。因此作为对话的一个初始化
        /// </summary>
        /// <param name="dialogParameters"></param>
        void OnDialogOpen(IDialogParameters dialogParameters);

        /// <summary>
        /// 确定（保存、执行）命令
        /// </summary>
        DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// 取消命令
        /// </summary>
        DelegateCommand CancelCommand { get; set; }
    }
}
