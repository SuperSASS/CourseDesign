using CourseDesign.Common.Classes;
using CourseDesign.ViewModels.Dialogs.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;

namespace CourseDesign.ViewModels.Dialogs
{
    internal class AddTextPlanViewModel : IDialogHostAware
    {
        #region 属性
        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public TextPlanClass AddTextPlan { get; set; }
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
                try
                {
                    if (string.IsNullOrWhiteSpace(AddTextPlan.Title) || string.IsNullOrWhiteSpace(AddTextPlan.Content)) // 计划的标题或内容为空
                        throw new Exception("计划要写好标题和内容啦_(:зゝ∠)_……"); // 返回错误提示
                    DialogParameters parameters = new DialogParameters();
                    parameters.Add("AddTextPlan", AddTextPlan); // 参数中添加
                    DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Yes, parameters));
                }
                catch { }
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

        /// <summary>
        /// 当刚打开这个对话时执行的操作【由于只是新增，直接生成AddTextPlan
        /// </summary>
        /// <param name="dialogParameters"></param>
        public void OnDialogOpen(IDialogParameters dialogParameters)   
        {
            AddTextPlan = new(0, false, null, null);
        }
    }
}
