using CourseDesign.ViewModels.Dialogs.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.ViewModels.Dialogs
{
    public class QueryViewModel : BindableBase, IDialogHostAware
    {
        #region 字段
        private string title;
        private string content;
        #endregion

        #region 属性

        /// <summary>
        /// 询问弹窗的标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// 询问弹窗的内容文本
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryViewModel()
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
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Yes, parameters));
            }
        }

        /// <summary>
        /// 取消命令
        /// </summary>
        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Cancel)); // 注意这里也要返回一个Cancel状态
        }

        public void OnDialogOpen(IDialogParameters dialogParameters)
        {
            try
            {
                // 从参数得到基本信息（Dialog的标题和内容）
                if (!dialogParameters.ContainsKey("Title") || !dialogParameters.ContainsKey("Content"))
                    throw new ArgumentException("内部错误(Dialog) - 未完整给出弹窗的基本参数");
                Title = dialogParameters.GetValue<string>("Title");
                Content = dialogParameters.GetValue<string>("Content");
            }
            catch
            { }
        }
    }
}
