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
        Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName="root");
    }
}
