using CourseDesign.Common.Events;
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
        #region 询问Query弹窗
        /// <summary>
        /// 展开询问窗口
        /// </summary>
        /// <param name="dialogService">指定的Dialog主机（服务）</param>
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
        #endregion

        #region 等待Loading弹窗
        /// <summary>
        /// 注册等待消息
        /// </summary>
        public static void RegisterLoadingDialog(this IEventAggregator aggregator, Action<LoadingModel> action)
        {
            aggregator.GetEvent<LoadingEvent>().Subscribe(action);
        }
        /// <summary>
        /// 发布消息，展开等待窗口
        /// </summary>
        public static void ShowLoadingDialog(this IEventAggregator aggregator, LoadingModel model)
        {
            aggregator.GetEvent<LoadingEvent>().Publish(model);
        }
        #endregion

        #region 底部浮动提示消息Message弹窗
        /// <summary>
        /// 注册底部浮动提示消息
        /// </summary>
        public static void RegisterMessageDialog(this IEventAggregator aggregator, Action<MessageModel> action, string filterName)
        {
            aggregator.GetEvent<MessageEvent>().Subscribe(action,
                ThreadOption.PublisherThread, // 推送事件的线程
                true, // 保持订阅事件
                (model) => { return model.Filter.Equals(filterName); } // 过滤器，用filterName过滤
                );
        }

        /// <summary>
        /// 发布消息，展开底部浮动提示消息弹窗
        /// </summary>
        /// <param name="aggregator">事件聚合器（若继承了DialogNavigationViewModel，则不用给这个）</param>
        /// <param name="message">发送的消息</param>
        /// <param name="filterName">过滤器（发送到哪去，主界面的为Main；登陆界面的为Login）</param>
        public static void ShowMessageDialog(this IEventAggregator aggregator, string message, string filterName)
        {
            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel() { Message = message, Filter = filterName });
        }
        #endregion
    }
}
