namespace CourseDesign.API.Context
{
    public class ImageTask : BaseEntity
    {
        public int TDoll_ID { get; set; }
        public bool Status { get; set; }
        public int UserID { get; set; }
    }
}
