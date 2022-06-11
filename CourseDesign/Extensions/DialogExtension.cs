﻿using CourseDesign.Common.Events;
using CourseDesign.Services.Dialog;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace CourseDesign.Extensions
{
    /// <summary>
    /// 用于提供各种弹窗展示服务的插件
    /// </summary>
    public static class DialogExtension
    {
        /// <summary>
        /// 展开询问窗口
        /// </summary>
        /// <param name="dialogService">指定的Dialog主机（服务），</param>
        /// <param name="title">询问窗口的标题</param>
        /// <param name="content">询问窗口的内容</param>
        /// <param name="dialogHostName">Dialog主机所展现的位置（默认为RootDialog，为全局对话）</param>
        /// <returns></returns>
        public static async Task<IDialogResult> ShowQueryDialog(this IDialogHostService dialogService, string title, string content, string dialogHostName = "RootDialog")
        {
            DialogParameters parameters = new() // 构造传递给Dialog的参数
            {
                { "Title", title },
                { "Content", content },
                { "dialogHostName", dialogHostName }
            };
            return await dialogService.ShowDialog("QueryView", parameters, dialogHostName);
        }

        /// <summary>
        /// 发送消息，展开等待窗口
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="model"></param>
        public static void ShowLoadingDialog(this IEventAggregator aggregator, LoadingModel model)
        {
            aggregator.GetEvent<LoadingEvent>().Publish(model);
        }

        /// <summary>
        /// 注册等待消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void Register(this IEventAggregator aggregator, Action<LoadingModel> action)
        {
            aggregator.GetEvent<LoadingEvent>().Subscribe(action);
        }

        public static void Register(this IEventAggregator aggregator, Action<MessageModel> action)
        {
            aggregator.GetEvent<MessageEvent>().Sb
        }
    }
}
