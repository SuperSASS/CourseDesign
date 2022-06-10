using CourseDesign.ViewModels.Dialogs.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace CourseDesign.ViewModels.Dialogs
{
    internal class AddTextPlanViewModel : IDialogHostAware
    {
        #region 属性
        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand {get; set;}
        public DelegateCommand CancelCommand { get; set;}
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public AddTextPlanViewModel()
        {
            // 初始化命令模块
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        /// <summary>
        /// 确认添加计划命令
        /// </summary>
        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters parameters = new DialogParameters();
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameters));
            }
        }

        /// <summary>
        /// 取消命令
        /// </summary>
        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Cancel));
        }

        public void OnDialogOpen(IDialogParameters dialogParameters)
        {
        }
    }
}
