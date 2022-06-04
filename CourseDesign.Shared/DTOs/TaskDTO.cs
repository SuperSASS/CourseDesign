namespace CourseDesign.Shared.DTOs
{
    /// <summary>
    /// Task计划数据实体
    /// </summary>
    public class TaskDTO : BaseDTO
    {
        public enum TypeEnum { text, image }; // 计划类型的枚举

        private TypeEnum type;
        private bool status;
        private int userId;

        /// <summary>
        /// 该计划属于什么类型
        /// </summary>
        public TypeEnum Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged(); }
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
