using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Common.Events
{
    /// <summary>
    /// 事件（消息）的实体类，里面加了可以区分推送到哪去订阅的标识。
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 弹窗是否启用（占据屏幕以等待）
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 过滤器，指定推送到哪里。
        /// </summary>
        public string Filter { get; set; }
    }
    /// <summary>
    /// 底部浮动消息事件
    /// </summary>
    public class MessageEvent : PubSubEvent<MessageModel>  { }
}
