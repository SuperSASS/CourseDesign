namespace CourseDesign.Shared.DTOs
{
    /// <summary>
    /// ImagePlan（计划列表的文字类计划）数据实体，继承于PlanDTO
    /// </summary>
    public class ImagePlanDTO : PlanDTO
    {
        private int TDoll_id;
        public int TDoll_ID
        {
            get { return TDoll_id; }
            set { TDoll_id = value; OnPropertyChanged(); }
        }
    }
}
