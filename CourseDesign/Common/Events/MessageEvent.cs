using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Common.Events
{
    /// <summary>
    /// 底部浮动消息事件
    /// </summary>
    public class MessageEvent : PubSubEvent<string>  { }
}
