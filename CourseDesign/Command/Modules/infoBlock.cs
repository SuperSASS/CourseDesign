using Prism.Mvvm;

namespace CourseDesign.Command.Module
{
    /// <summary>
    /// 首页信息类：
    /// <para>用于在首页展现个人信息。</para>
    /// </summary>
    public class infoBlock : BindableBase
    {
        private string icon; // 信息栏图标
        private string title; // 信息栏标题
        private string content; // 信息栏内容
        private string toolTip; // 信息栏悬浮文本
        private string target; // 信息栏触发目标

        public string Icon { get { return icon; } set { icon = value; } }
        public string Title { get { return title; } set { title = value; } }
        public string Content { get { return content; } set { content = value; } }
        public string ToolTip { get { return toolTip; } set { toolTip = value; } }
        public string Target { get { return target; } set { target = value; } }

        /// <summary>
        /// 首页信息类 - 构造函数（图标，标题，内容，触发目标）
        /// </summary>
        /// <param name="icon"> 图标 </param>
        /// <param name="title"> 标题 </param>
        /// <param name="content"> 内容 </param>
        /// <param name="toolTip"> 悬浮提示 </param>
        /// <param name="target"> 跳转视图（对应视图名 </param>
        public infoBlock(string icon, string title, string content, string toolTip, string target)
        {
            Icon = icon;
            Title = title;
            Content = content;
            ToolTip = toolTip;
            Target = target;
        }
    }
}
