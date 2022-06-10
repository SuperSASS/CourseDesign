using CourseDesign.Common.Classes.Bases;
using CourseDesign.Common.Module;
using CourseDesign.Services.API.Interfaces;
using CourseDesign.Services.Dialog;
using CourseDesign.ViewModels.Bases;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using static CourseDesign.Context.LoginUserContext;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CourseDesign.Common.Classes;

// 首页的实现逻辑
namespace CourseDesign.ViewModels
{
    internal class IndexViewModel : NavigationViewModel
    {
        #region 字段
        // 服务字段
        private readonly ITextPlanService TextService;       // 文本类计划服务
        private readonly IImagePlanService ImageService; // 图片类计划服务
        private readonly IDialogHostService DialogService;   // 弹窗服务
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
        public IndexViewModel(IContainerProvider containerProvider, IDialogHostService dialogService) : base(containerProvider)
        {
            // 注入弹窗服务
            DialogService = dialogService;
            TextService = containerProvider.Resolve<ITextPlanService>();
            ImageService = containerProvider.Resolve<IImagePlanService>();
            // 初始化可视数据模块
            InfoBlocks = new ObservableCollection<InfoBlock>();
            PlanLists = new ObservableCollection<PlanBase>();
            // 初始化命令模块
            ExecCommand = new DelegateCommand<string>(Exec);
        }

        /// <summary>
        /// 当导航到该页面时执行的方法
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            ShowLoadingDialog(true);
            CreateInfoBlocks();
            CreatePlansLists();
            ShowLoadingDialog(false);
        }

        public void Exec(string cmd)
        {
            switch (cmd)
            {
                case "新增常规计划": AddTextPlan(); break;
                default: break;
            }
        }

        async void AddTextPlan()
        {
            var dialogResult = await DialogService.ShowDialog("AddTextPlanView", null);
            if (dialogResult.Result != ButtonResult.Yes) return;
        }

        #region 内部方法
        /// <summary>
        /// 生成展示在首页的的信息块列表
        /// </summary>
        public void CreateInfoBlocks()
        {
            InfoBlocks.Clear();
            InfoBlocks.Add(new InfoBlock("PercentCircleOutline", "收集情况", "100%", "跳转到图鉴页面", ""));
            InfoBlocks.Add(new InfoBlock("CheckboxMarkedCirclePlusOutline", "计划完成情况", "50%", "跳转到计划列表页面", ""));
        }

        /// <summary>
        /// 生成展示在首页的计划列表
        /// </summary>
        private async void CreatePlansLists()
        {
            PlanLists.Clear();
            foreach (var waitTask in WaitTasks)
                await waitTask;
            foreach (var item in UserPlans)
                if (item is TextPlanClass)
                    PlanLists.Add(item);
        }


        #endregion
    }
}
