namespace CourseDesign.Shared.DTOs
{
    /// <summary>
    /// Plan计划数据实体基类
    /// </summary>
    public class PlanDTO : BaseDTO
    {
        public enum PlanType { text, image }; // 计划类型的枚举

        private PlanType type;
        private bool status;
        private int userId;

        /// <summary>
        /// 该计划属于什么类型
        /// </summary>
        public PlanType Type
        {
            get { return type; }
            set { type = value; }
        }
        /// <summary>
        /// 该计划状态
        /// </summary>
        public bool Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 该计划属于哪个用户ID
        /// </summary>
        public int UserID
        {
            get { return userId; }
            set { userId = value; }
        }
    }
}
