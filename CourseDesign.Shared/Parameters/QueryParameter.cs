using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Shared.Parameters
{
    public class QueryParameter
    {
        public string Search { get; set; } // 查询的值
        public string Field { get; set; }  // 查询的字段
        public int PageIndex { get; set; } // 查询的页号
        public int PageSize { get; set; }  // 查询的个数
    }
}
