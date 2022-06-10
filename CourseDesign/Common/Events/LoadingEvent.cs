using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Common.Events
{
    public class LoadingModel
    {
        /// <summary>
        /// 弹窗是否启用（占据屏幕以等待）
        /// </summary>
        public bool IsOpen { get; set; }
    }

    /// <summary>
    /// 等待操作事件(Event)
    /// </summary>
    internal class LoadingEvent : PubSubEvent<LoadingModel> { }
}
