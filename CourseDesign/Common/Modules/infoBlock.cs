using Prism.Mvvm;
using System;

namespace CourseDesign.Common.Module
{
    /// <summary>
    /// 首页信息类：
    /// <para>用于在首页展现个人信息。</para>
    /// </summary>
    public class InfoBlock : BindableBase
    {
        private string icon; // 信息栏图标
        private string title; // 信息栏标题
        private double complete; // 计划已完成（人形已获取）数量
        private double all; // 所有计划（人形）数量
        private string ratio_integer; // 信息栏计算比率 - 整数部分
        private string ratio_decimal; // 信息栏计算比率 - 小数部分
        private string toolTip; // 信息栏悬浮文本
        private string target; // 信息栏触发目标

        public string Icon { get { return icon; } set { icon = value; } }
        public string Title { get { return title; } set { title = value; } }
        public double Complete { get { return complete; } set { complete = value; CalcRatio(); RaisePropertyChanged(); } }
        public double All { get { return all; } set { all = value; CalcRatio(); RaisePropertyChanged(); } }
        public string Ratio_Integer { get { return ratio_integer; } set { ratio_integer = value; RaisePropertyChanged(); } }
        public string Ratio_Decimal { get { return ratio_decimal; } set { ratio_decimal = value; RaisePropertyChanged(); } }
        public string ToolTip { get { return toolTip; } set { toolTip = value; } }
        public string Target { get { return target; } set { target = value; } }

        /// <summary>
        /// 首页信息类 - 构造函数（图标，标题，内容，触发目标）
        /// </summary>
        /// <param name="icon"> 图标 </param>
        /// <param name="title"> 标题 </param>
        /// <param name="content"> 内容 </param>
        /// <param name="toolTip"> 悬浮提示 </param>
        /// <param name="target"> 跳转视图（对应视图名 </param>
        public InfoBlock(string icon, string title, double complete, double all, string toolTip, string target)
        {
            Icon = icon;
            Title = title;
            Complete = complete;
            All = all;
            CalcRatio();
            ToolTip = toolTip;
            Target = target;
        }

        private void CalcRatio()
        {
            string Ratio = All == 0 ? "--.--" : (Math.Round(Complete / All, 4) * 100).ToString("00.00");
            Ratio_Integer = Ratio.Substring(0, 3);
            Ratio_Decimal = Ratio.Substring(3, 2);
        }
    }
}
