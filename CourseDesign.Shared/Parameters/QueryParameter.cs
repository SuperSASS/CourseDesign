namespace CourseDesign.Shared.Parameters
{
    public class QueryParameter
    {
        public int UserId { get; set; }  // 查询内容属于哪个用户
        public string Search { get; set; } // 查询的值【若为null，代表全部
        public string Field { get; set; }  // 查询的字段
        public int PageIndex { get; set; } // 查询的页号
        public int PageSize { get; set; }  // 查询的个数

        // 注意！不能用构造函数，否则报错
    }
}
