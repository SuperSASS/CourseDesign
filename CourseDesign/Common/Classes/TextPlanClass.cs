namespace CourseDesign.Common.Classes
{
    /// <summary>
    /// 文字计划类
    /// <para>继承于计划基类<c>PlansBase</c>，具有五个属性：</para>
    /// <list type="number">
    /// <item><c>ID</c> - 计划编号</item>
    /// <item><c>Type</c> - 计划类型</item>
    /// <item><c>Status</c> - 计划状态</item>
    /// <item><c>title</c> - 计划标题</item>
    /// <item><c>content</c> - 计划内容</item>
    /// </list>
    /// </summary>
    public class TextPlanClass : PlanBase
    {
        private string title;
        private string content;

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

        /// <summary>
        /// 文字计划类的构造函数，计划种类Type在这里传text给基类
        /// </summary>
        /// <param name="id">计划ID（基类的属性）</param>
        /// <param name="status">计划状态（0表示未完成，1表示已完成）（基类的属性）</param>
        /// <param name="title">计划标题</param>
        /// <param name="content">计划内容</param>
        public TextPlanClass(int id, bool status, string title, string content) : base(id, PlanType.Text, status)
        {
            Title = title;
            Content = content;
        }
    }
}
