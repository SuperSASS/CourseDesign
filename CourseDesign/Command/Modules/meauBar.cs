using Prism.Mvvm;

namespace CourseDesign.Command.Modules
{
    /// <summary>
    /// 菜单栏导航类：
    /// <para>用于在菜单栏创建导航。</para>
    /// </summary>
    public class MeauBar : BindableBase
    {
        private string icon; // 导航图标
        private string title; // 导航名称
        private string nameSpace;  // 导航命令空间
        public string Icon { get { return icon; } set { icon = value; } }
        public string Title { get { return title; } set { title = value; } }
        public string NameSpace { get { return nameSpace; } set { nameSpace = value; } }

        /// <summary>
        /// 菜单栏导航类 - 构造函数（图标，标题，命名空间）
        /// </summary>
        /// <param name="icon"> 图标 </param>
        /// <param name="title"> 标题 </param>
        /// <param name="nameSpace"> 命名空间（对应视图名） </param>
        public MeauBar(string icon, string title, string nameSpace)
        {
            Icon = icon;
            Title = title;
            NameSpace = nameSpace;
        }
    }
}
