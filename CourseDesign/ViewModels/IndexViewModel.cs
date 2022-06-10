using CourseDesign.Common.Classes;
using CourseDesign.Common.Module;
using CourseDesign.Services.Dialog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

// 首页的实现逻辑
namespace CourseDesign.ViewModels
{
    internal class IndexViewModel : BindableBase
    {
        #region 字段
        // 服务字段
        private readonly IDialogHostService DialogService; // 弹窗服务
        // 属性字段
        private ObservableCollection<InfoBlock> infoBlocks;
        private ObservableCollection<PlanBase> planLists;
        #endregion

        #region 属性
        public ObservableCollection<InfoBlock> InfoBlocks
        {
            get { return infoBlocks; }
            set { infoBlocks = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<PlanBase> PlanLists
        {
            get { return planLists; }
            set { planLists = value; }
        }
        /// <summary>
        /// 首页的执行命令的总方法
        /// </summary>
        public DelegateCommand<string> ExecCommand { get; private set; }
        #endregion


        /// <summary>
        /// IndexView VM 的构造函数：
        /// <para>完成菜单栏、导航命令、后退命令、返回主页命令的初始化</para>
        /// </summary>
        public IndexViewModel(IDialogHostService dialogService)
        {
            // 注入弹窗服务
            DialogService = dialogService;
            // 初始化可视数据模块
            InfoBlocks = new ObservableCollection<InfoBlock>();
            PlanLists = new ObservableCollection<PlanBase>();
            // 初始化命令模块
            ExecCommand = new DelegateCommand<string>(Exec);
        }

        public void Exec(string cmd)
        {
            switch (cmd)
            {
                case "新增常规计划": DialogService.ShowDialog("AddTextPlanView", null); break;
                default: break;
            }
        }

        /// <summary>
        /// 生成测试用的信息块列表
        /// </summary>
        //public void TEST_CreateInfoBlocks()
        //{
        //    InfoBlocks.Add(new InfoBlock("PercentCircleOutline", "收集情况", "100%", "跳转到图鉴页面", ""));
        //    InfoBlocks.Add(new InfoBlock("CheckboxMarkedCirclePlusOutline", "计划完成情况", "50%", "跳转到计划列表页面", ""));
        //}

        /// <summary>
        /// 生成测试用的计划列表
        /// </summary>
        //public void TEST_CreatePlansLists()
        //{
        //    for (int i = 1; i <= 10; i++)
        //        PlanLists.Add(new TextPlanClass(i, false, "测试" + i, "计划内容……"));
        //}
    }
}
