using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Context
{
    public class ContextWaitTasks
    {
        private static List<Task> waitTasks;
        /// <summary>
        /// 加载完全局的上下文，所需要等待的任务组
        /// </summary>
        public static List<Task> WaitTasks
        {
            get { return waitTasks; }
            private set { waitTasks = value; }
        }

        public ContextWaitTasks()
        {
            WaitTasks = new();
        }
    }
}
