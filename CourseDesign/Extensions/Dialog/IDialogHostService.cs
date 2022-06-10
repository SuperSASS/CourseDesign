using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Extensions.Dialog
{
    /// <summary>
    /// 为了使得Prism里的DialogService支持MaterialDesign里的Dialog样式，需要自写扩展接口
    /// </summary>
    public interface IDialogHostService : IDialogService
    {
        // 可以看到这里的基本服务只有ShowDialog
        // name - 展示的Dialog窗口的名称（如""）
        Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName="RootDialog");
    }
}
