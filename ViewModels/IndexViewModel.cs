using CourseDesign.Command.Module;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 首页的实现逻辑
namespace CourseDesign.ViewModels
{
    internal class IndexViewModel : BindableBase
    {

        private ObservableCollection<infoBlock> infoBlocks;
        public ObservableCollection<infoBlock> InfoBlocks
        {
            get { return infoBlocks; }
            set { infoBlocks = value;  RaisePropertyChanged(); }
        }

        /// <summary>
        /// IndexView VM 的构造函数：
        /// <para>完成菜单栏、导航命令、后退命令、返回主页命令的初始化</para>
        /// </summary>
        public IndexViewModel()
        {
            InfoBlocks = new ObservableCollection<infoBlock>();
            CreateInfoBlocks();
        }


        /// <summary>
        /// 创建信息块列表
        /// </summary>
        public void CreateInfoBlocks()
        {
            InfoBlocks.Add(new infoBlock("Percent", "收集情况", "100%", ""));
            InfoBlocks.Add(new infoBlock("Percent", "收集情况", "100%", ""));
        }
    }
}
