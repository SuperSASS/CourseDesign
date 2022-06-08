namespace CourseDesign.Shared.Parameters
{
    /// <summary>
    /// APP向API发送查询请求时，所用的参数列表
    /// </summary>
    public class GETParameter
    {
        /// <summary>
        /// 查询内容属于哪个用户，由用户ID表示（为空的情况只有在图鉴查询会遇到）
        /// </summary>
        public int? user_id { get; set; }
        /// <summary>
        /// 查询的内容（若为null，代表全查询）
        /// </summary>
        public string search { get; set; }
        /// <summary>
        /// 查询的字段，该属性不会为空
        /// </summary>
        public string field { get; set; }  // 【数据库里不分字段属性，所以这里叫字段，但实际上在C#里是属性
        /// <summary>
        /// 分页结果，查询的页号
        /// </summary>
        public int? page_index { get; set; }
        /// <summary>
        /// 分页结果，每页的个数
        /// </summary>
       public int? page_size { get; set; }  // 查询的个数

        // 注意！不能用构造函数，否则报错【应该是没考虑所有情况的构造，所以干脆就不写
        // 注：有关元组Status状态（完成/拥有），将放在应用层处理，而不是服务器层处理：服务器层始终返回满足**一条**要求的
    }
}
