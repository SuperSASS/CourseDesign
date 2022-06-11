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
using CourseDesign.Shared;
using CourseDesign.Services.API.ClassServices;
using CourseDesign.Shared.DTOs;
using CourseDesign.Context;
using CourseDesign.Extensions;

// 首页的实现逻辑
namespace CourseDesign.ViewModels
{
    internal class IndexViewModel : NavigationViewModel
    {
        #region 字段
        private readonly IRegionManager regionManager; // 区域控制器
        private IRegionNavigationJournal journal; // 区域导航日志
        // 服务字段
        private readonly ITextPlanService TextService;       // 文本类计划服务
        private readonly IImagePlanService ImageService; // 图片类计划服务
        private readonly ITDollService TDollService; // 人形相关服务
        private readonly IDialogHostService DialogService;   // 弹窗服务
        // 属性字段
        private ObservableCollection<InfoBlock> infoBlocks;
        private ObservableCollection<PlanBase> planLists;
        // 控件绑定命令字段
        public DelegateCommand<InfoBlock> NavigationCommand { get; private set; } // 从UI层传递MenuBars到这个导航命令
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
        public DelegateCommand<PlanBase> CompletePlanCommand { get; private set; }
        #endregion

        /// <summary>
        /// IndexView VM 的构造函数：
        /// <para>完成菜单栏、导航命令、后退命令、返回主页命令的初始化</para>
        /// </summary>
        public IndexViewModel(IRegionManager regionManager, IContainerProvider containerProvider) : base(containerProvider)
        {
            this.regionManager = regionManager;
            // 从容器得到注入的API
            TextService = containerProvider.Resolve<ITextPlanService>();
            ImageService = containerProvider.Resolve<IImagePlanService>();
            TDollService = containerProvider.Resolve<ITDollService>();
            // 注入弹窗服务
            DialogService = containerProvider.Resolve<IDialogHostService>();
            // 初始化可视数据模块
            InfoBlocks = new ObservableCollection<InfoBlock>();
            PlanLists = new ObservableCollection<PlanBase>();
            // 初始化命令模块
            ExecCommand = new DelegateCommand<string>(Exec);
            CompletePlanCommand = new DelegateCommand<PlanBase>(CompletePlan);
            NavigationCommand = new DelegateCommand<InfoBlock>(Navigate);
        }

        /// <summary>
        /// 当导航到该页面时执行的方法
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            // 加载页面数据 - InfoBlocks和PlansLists
            ShowLoadingDialog(true);
            CreateInfoBlocks();
            CreatePlanLists();
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

        /// <summary>
        /// 从主页中增添文字计划，会先发出弹窗输入文字计划内容
        /// </summary>
        async void AddTextPlan()
        {
            var dialogResult = await DialogService.ShowDialog("AddTextPlanView", null);
            if (dialogResult.Result != ButtonResult.Yes) return;
            try
            {
                TextPlanClass textPlan = dialogResult.Parameters.GetValue<TextPlanClass>("AddTextPlan");
                if (string.IsNullOrWhiteSpace(textPlan.Title) || string.IsNullOrWhiteSpace(textPlan.Content)) // 计划的标题或内容为空
                    throw new Exception("计划要写好标题和内容啦_(:зゝ∠)_……"); // 返回错误提示
                ShowLoadingDialog(true);
                var updateResponse = await TextService.Add(textPlan.ConvertDTO(textPlan, LoginUserID));
                if (updateResponse.Status == APIStatusCode.Success)
                {
                    textPlan.ID = updateResponse.Result.ID; // 别忘了记录数据库ID！
                    UserPlans.Add(textPlan);
                    PlanLists.Insert(0, textPlan);
                }
                CreateInfoBlocks(); // 重新更新信息块
            }
            catch { }
            finally
            {
                ShowLoadingDialog(false);
            }
        }

        /// <summary>
        /// 在主页中完成了某计划
        /// </summary>
        /// <param name="plan">计划基类</param>
        private async void CompletePlan(PlanBase plan)
        {
            try
            {
                APIStatusCode APIResponseStatus;
                ShowLoadingDialog(true);
                if (plan is TextPlanClass text)
                {
                    text.Status = true;
                    APIResponseStatus = (await TextService.Update(text.ConvertDTO(text, LoginUserID))).Status;
                }
                else
                {
                    var image = plan as ImagePlanClass;
                    image.Status = true;
                    APIResponseStatus = (await ImageService.Update(image.ConvertDTO(image, LoginUserID))).Status;
                }
                if (APIResponseStatus != APIStatusCode.Success)
                    throw new Exception("诶，好像改不了这个任务的状态，待会再试试呢【……");
                GetPlan(plan.ID).Status = plan.Status; // 将本地的状态也更改
                UserPlansComplete++; // 用户完成计划数++
                if (plan is ImagePlanClass imagePlan) // 如果是图片类计划，还要额外获得人形
                {
                    APIResponseStatus = (await TDollService.AddUserObtain(new TDollObtainDTO() { UserID = LoginUserID, TDollID = imagePlan.TDoll_ID })).Status;
                    if (APIResponseStatus != APIStatusCode.Success)
                        throw new Exception("内部错误 - 用户那里添加不了获取的人形……");
                    UserTDolls.Add(imagePlan.TDoll_ID); // 注意：这里要修改本地上下文
                }
                CreatePlanLists(); // 重新生成显示内容
                CreateInfoBlocks(); // 重新更新信息块
                aggregator.ShowMessageDialog("任务已完成！");
            }
            finally
            {
                ShowLoadingDialog(false);
            }
        }

        /// <summary>
        /// 用于驱动页面的切换的实现方法
        /// </summary>
        /// <param name="obj">当注册的导航MenuBar响应后，自动调用该方法，并作为参数obj</param>
        private void Navigate(InfoBlock obj)
        {
            if (obj != null && !string.IsNullOrWhiteSpace(obj.Target))
                regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target, back => { journal = back.Context.NavigationService.Journal; });
        }

        #region 内部方法
        /// <summary>
        /// 生成展示在首页的的信息块列表
        /// </summary>
        private async void CreateInfoBlocks()
        {
            InfoBlocks.Clear();
            foreach (var waitTask in WaitTasks)
                await waitTask;
            // 人形收集情况
            double TDolls_All = TDollsContext.AllTDolls.Count;
            double TDolls_User = UserTDolls.Count;
            InfoBlocks.Add(new InfoBlock("PercentCircleOutline", "人形收集情况", TDolls_User, TDolls_All, "跳转到图鉴页面","ListView"));
            // 计划完成情况
            double Plans_All = UserPlans.Count;
            double Plans_Complete = UserPlansComplete;
            InfoBlocks.Add(new InfoBlock("CheckboxMarkedCirclePlusOutline", "计划完成情况", Plans_Complete, Plans_All, "跳转到计划列表页面", "PlanView"));
        }

        /// <summary>
        /// 生成展示在首页的计划列表
        /// </summary>
        private async void CreatePlanLists()
        {
            PlanLists.Clear();
            // 利用异步的方法，等待UserContext里的所有数据加载完成（即便不成功）再加载之后的数据
            foreach (var waitTask in WaitTasks)
                await waitTask;
            foreach (var item in UserPlans)
                if (item.Status == false) // 添加未完成的TextPlan到主页展示
                    PlanLists.Add(item);
        }
        #endregion
    }
}
