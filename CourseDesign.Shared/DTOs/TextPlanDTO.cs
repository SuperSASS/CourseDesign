namespace CourseDesign.Shared.DTOs
{
    /// <summary>
    /// TextPlan（计划列表的文字类计划）数据实体，继承于PlanDTO
    /// </summary>
    public class TextPlanDTO : PlanDTO
    {
        private string title;
        private string content;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }
        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(); }
        }
    }
}
