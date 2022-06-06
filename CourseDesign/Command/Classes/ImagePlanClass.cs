using CourseDesign.Context;

namespace CourseDesign.Command.Classes
{
    /// <summary>
    /// 图片计划类，特指计划打捞某位战术人形。
    /// <para>继承与计划基类<c>PlansBase</c>，具有四个属性：</para>
    /// <list type="number">
    /// <item><c>ID</c> - 计划编号</item>
    /// <item><c>Type</c> - 计划类型</item>
    /// <item><c>Status</c> - 计划状态</item>
    /// <item><c>TDoll_ID</c> - 计划打捞战术人形的ID</item>
    /// </list>
    public class ImagePlanClass : PlanBase
    {
        private int tDoll_ID;
        public TDollClass TDoll { get; set; } // 根据ID找到的人形类

        /// <summary>
        /// 图片计划类的构造函数，计划种类Type在这里传image给基类
        /// </summary>
        /// <param name="id">计划ID（基类的属性）</param>
        /// <param name="status">计划状态（0表示未完成，1表示已完成）（基类的属性）</param>
        /// <param name="TDoll_id">计划打捞战术人形的ID</param>
        public ImagePlanClass(int id, bool status, int tDoll_ID) : base(id, TypeEnum.image, status)
        {
            this.tDoll_ID = tDoll_ID;
            TDoll = TDollsContext.GetTDoll(this.tDoll_ID);
        }
    }
}
