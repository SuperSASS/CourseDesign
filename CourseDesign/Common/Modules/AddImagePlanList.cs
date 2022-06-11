using CourseDesign.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.Common.Modules
{
    /// <summary>
    /// 用于在“添加人形获取计划”列表中展示的类
    /// </summary>
    /// 其实跟得到用户所拥有的人形区别在于：这个还要考虑是否在计划里，在计划里的也不能选！
    public class AddImagePlanList
    {
        private TDollClass tDoll;
        private bool isChecked;
        private bool isDefaultEnabled; // 需要把选项是否默认启用给单列出来，用来判断用户的选择是先定的还是后选的

        /// <summary>
        /// 直接用TDoll代表该人形所有信息
        /// </summary>
        public TDollClass TDoll
        {
            get { return tDoll; }
            private set { tDoll = value; }
        }
        /// <summary>
        /// 判断是否拥有，然后确定是否默认选中（选中后IsEnabled要改为false）
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        /// <summary>
        /// 是否默认启用可选状态
        /// </summary>
        public bool IsDefaultEnabled
        {
            get { return isDefaultEnabled; }
            set { isDefaultEnabled = value; }
        }

        /// <summary>
        /// 展示在“添加人形获取计划”页面中的可选择的人形数据源
        /// </summary>
        /// <param name="tDoll">对应的<see cref="TDollClass"/>类型人形数据</param>
        /// <param name="isDefaultChecked">是否默认被勾选，根据用户是否已经拥有该人形决定</param>
        public AddImagePlanList(TDollClass tDoll, bool isDefaultChecked)
        {
            TDoll = tDoll;
            IsChecked = isDefaultChecked;
            IsDefaultEnabled = !isDefaultChecked; // 在构造函数指定默认是否可选，如果刚开始勾选了则默认不可选。
        }

    }
}
