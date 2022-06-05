namespace CourseDesign.API.Context
{
    // 文本型计划关系表
    public class TextPlan : BaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 完成状态（1为完成）
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 所属用户的ID
        /// </summary>
        public int UserID { get; set; }
    }
}
