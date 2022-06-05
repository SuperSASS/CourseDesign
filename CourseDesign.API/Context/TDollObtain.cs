namespace CourseDesign.API.Context
{
    // 玩家人形解锁情况表
    public class TDollObtain : BaseEntity
    {
        /// <summary>
        /// 对应的人形ID，为外键
        /// </summary>
        public int TDollID { get; set; }
        /// <summary>
        /// 对应的用户ID，为外键
        /// </summary>
        public int UserID { get; set; }
    }
}
