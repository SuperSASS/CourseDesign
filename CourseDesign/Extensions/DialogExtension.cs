using CourseDesign.Common.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Extensions
{
    public static class DialogExtension
    {
        // 询问窗口
        public static async Task<IDialogResult> ShowQuestion(this IDialogHostService dialogHost, string title, string content, string dialogHostName = "RootDialog")
        {
            DialogParameters parameters= new();
            parameters.Add("Title",title);
            parameters.Add("Content",content);
            parameters.Add("dialogHostName",dialogHostName);
            return await dialogHost.ShowDialog("MessageView", parameters, dialogHostName);
        }

        /// <summary>
        /// 发送消息，展开等待窗口
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="model"></param>
        public static void Loading(this IEventAggregator aggregator, LoadingModel model)
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
    }
}
