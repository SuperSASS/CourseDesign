using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Extensions.Dialog
{
    // 这是用给各种Dialog的VM继承的，如AddTextPlanViewModel
    public interface IDialogHostAware // 此处并不需要继承IDialogAware，因为并不需要那么多方法，只用我们所需要的方法即可。
    {
        /// <summary>
        /// 所属DialogHost的主机名称
        /// </summary>
        string DialogHostName { get; set; }

        /// <summary>
        /// 打开过程中执行这里，并接受参数
        /// </summary>
        /// <param name="dialogParameters"></param>
        void OnDialogOpen(IDialogParameters dialogParameters);

        /// <summary>
        /// 确定命令
        /// </summary>
        DelegateCommand SaveCommand { get; set;}

        /// <summary>
        /// 取消命令
        /// </summary>
        DelegateCommand CancelCommand { get; set; }
    }
}
