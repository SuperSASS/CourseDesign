namespace CourseDesign.API.Context
{
    public class TextTask : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public int UserID { get; set; }
    }
}
