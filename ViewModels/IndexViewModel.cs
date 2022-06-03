using CourseDesign.Command.Classes;
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
            set { infoBlocks = value; RaisePropertyChanged(); }
        }


        // TODO: 这里没有处理多态情况，因为要等数据库api接口实现
        private ObservableCollection<TasksBase> taskLists;

        public ObservableCollection<TasksBase> TaskLists
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
            TaskLists = new ObservableCollection<TasksBase>();
            TEST_CreateInfoBlocks();
            TEST_CreateTasksLists();
        }


        /// <summary>
        /// 生成测试用的信息块列表
        /// </summary>
        public void TEST_CreateInfoBlocks()
        {
            InfoBlocks.Add(new infoBlock("PercentCircle", "收集情况", "100%", "跳转到图鉴页面", ""));
            InfoBlocks.Add(new infoBlock("CheckboxMarkedCirclePlusOutline", "任务完成情况", "50%", "跳转到任务列表页面", ""));
        }

        /// <summary>
        /// 生成测试用的任务列表
        /// </summary>
        public void TEST_CreateTasksLists()
        {
            for (int i = 1; i <= 10; i++)
                TaskLists.Add(new TextTasksClass(i, false, "测试" + i, "任务内容……"));
        }
    }
}
