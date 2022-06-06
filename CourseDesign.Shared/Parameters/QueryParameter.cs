namespace CourseDesign.Shared.Parameters
{
    /// <summary>
    /// APP向API发送查询请求时，所用的参数列表
    /// </summary>
    public class QueryParameter
    {
        public int user_id { get; set; }  // 查询内容属于哪个用户
        public string search { get; set; } // 查询的值【若为null，代表全部
        public string field { get; set; }  // 查询的字段【数据库里不分，但实际上在C#里是属性
        public int page_index { get; set; } // 查询的页号
        public int page_size { get; set; }  // 查询的个数

        // 注意！不能用构造函数，否则报错【应该是没考虑所有情况的构造，所以干脆就不写
    }
}
