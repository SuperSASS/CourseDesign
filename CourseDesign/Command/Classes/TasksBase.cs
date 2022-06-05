namespace CourseDesign.Command.Classes
{
    /// <summary>
    /// 计划列表的基类
    /// <para>具有三个属性：</para>
    /// <list type="number">
    /// <item><c>ID</c> - 计划编号</item>
    /// <item><c>Type</c> - 计划类型</item>
    /// <item><c>Status</c> - 计划状态，1代表完成</item>
    /// </list>
    /// </summary>
    internal class PlansBase
    {
        internal enum TypeEnum { text, image }; // 计划类型的枚举

        protected int id;        // 计划编号
        protected TypeEnum type; // 计划类型
        protected bool status;   // 计划状态，1代表完成
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public TypeEnum Type
        {
            get { return type; }
            set { type = value; }
        }
        public bool Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// 计划类的构造函数
        /// </summary>
        /// <param name="id">计划ID</param>
        /// <param name="type">计划种类（目前只有2类：图片类</param>
        /// <param name="status">计划状态（0表示未完成，1表示已完成）</param>
        public PlansBase(int id, TypeEnum type, bool status)
        {
            ID = id;
            Type = type;
            Status = status;
        }
    }
}
