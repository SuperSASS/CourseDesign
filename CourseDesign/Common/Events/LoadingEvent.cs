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
        public bool IsOpen { get; set; }
    }
    /// <summary>
    /// 等待操作事件
    /// </summary>
    internal class LoadingEvent : PubSubEvent<LoadingModel>
    {
    }
}
