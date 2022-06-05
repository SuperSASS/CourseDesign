using System;

namespace CourseDesign.API.Context
{
    /// <summary>
    /// 数据库各表的基类
    /// </summary>
    public class BaseEntity // 记录：生成数据库时，只能用属性！
    {
        /// <summary>
        /// ID，不同最终的继承类有不同的ID序列
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 元组创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 元组更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}
