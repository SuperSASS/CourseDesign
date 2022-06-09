using CourseDesign.Common.Classes;
using CourseDesign.Common.Module;
using Prism.Mvvm;
using System.Collections.ObjectModel;

// 首页的实现逻辑
namespace CourseDesign.ViewModels
{
    internal class IndexViewModel : BindableBase
    {

        private ObservableCollection<infoBlock> infoBlocks;
        public ObservableCollection<infoBlock> InfoBlocks
        {
            get { return infoBlocks; }
            set { infoBlocks = value; RaisePropertyChanged(); }
        }

        // TODO: 这里没有处理多态情况，因为要等数据库api接口实现
        private ObservableCollection<PlanBase> taskLists;

        public ObservableCollection<PlanBase> PlanLists
        {
            get { return taskLists; }
            set { taskLists = value; }
        }


        /// <summary>
        /// IndexView VM 的构造函数：
        /// <para>完成菜单栏、导航命令、后退命令、返回主页命令的初始化</para>
        /// </summary>
        public IndexViewModel()
        {
            InfoBlocks = new ObservableCollection<infoBlock>();
            PlanLists = new ObservableCollection<PlanBase>();
            TEST_CreateInfoBlocks();
            TEST_CreatePlansLists();
        }

        /// <summary>
        /// 生成测试用的信息块列表
        /// </summary>
        public void TEST_CreateInfoBlocks()
        {
            InfoBlocks.Add(new infoBlock("PercentCircleOutline", "收集情况", "100%", "跳转到图鉴页面", ""));
            InfoBlocks.Add(new infoBlock("CheckboxMarkedCirclePlusOutline", "计划完成情况", "50%", "跳转到计划列表页面", ""));
        }

        /// <summary>
        /// 生成测试用的计划列表
        /// </summary>
        public void TEST_CreatePlansLists()
        {
            for (int i = 1; i <= 10; i++)
                PlanLists.Add(new TextPlanClass(i, false, "测试" + i, "计划内容……"));
        }
    }
}
