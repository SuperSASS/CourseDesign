using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Services.Dialog
{
    /// <summary>
    /// 为了使得Prism里的DialogService支持MaterialDesign里的Dialog样式，需要自写扩展接口
    /// </summary>
    public interface IDialogHostService : IDialogService
    {
        // 可以看到这里的基本服务只有ShowDialog
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="name">对话模块名（如:）</param>
        /// <param name="parameters">给对话模块的参数</param>
        /// <param name="dialogHostName">弹出到View中的哪里（默认为RootDialog，是全局弹窗，在MainWindoe.xaml可看到）</param>
        /// <returns></returns>
        Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "RootDialog");
    }
}
