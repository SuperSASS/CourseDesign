namespace CourseDesign.API.Context
{
    // 图片类计划关系表
    public class ImagePlan : BaseEntity
    {
        /// <summary>
        /// 对应的人形ID
        /// </summary>
        public int TDoll_ID { get; set; }
        /// <summary>
        /// 完成状态（1是完成）
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 该计划所述用户ID
        /// </summary>
        public int UserID { get; set; }
    }
}
