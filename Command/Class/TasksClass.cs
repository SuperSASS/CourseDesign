using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: 可以在此之上创建个基类，这样就能满足继承了……
namespace CourseDesign.Command.Class
{
    /// <summary>
    /// 任务类
    /// <para>具有三个属性：</para>
    /// <list type="number">
    /// <item><c>ID</c> - 任务编号</item>
    /// <item><c>Title</c> - 任务标题</item>
    /// <item><c>Content</c> - 任务内容</item>
    /// <item><c>Status</c> - 任务状态</item>
    /// </list>
    /// </summary>
    internal class TasksClass
    {
        private int id;
        private string title;
        private string content;
        private bool status;

        public bool Status
        {
            get { return status; }
            set { status = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 任务类的构造函数
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <param name="content">任务内容</param>
        /// <param name="statu">任务状态（0表示未完成，1表示已完成）</param>
        public TasksClass(int id, string title, string content, bool statu)
        {
            ID = id;
            Title = title;
            Content = content;
            Status = status;
        }
    }
}
