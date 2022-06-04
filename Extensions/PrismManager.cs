using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Extensions
{
    /// <summary>
    /// Prism管理插件：
    /// <para>目前只用于静态地指定区域名称</para>
    /// </summary>
    internal class PrismManager
    {
        public static readonly string MainViewRegionName = "MainViewRegion";
        public static readonly string SettingViewRegionName = "SettingViewRegion";
    }
}