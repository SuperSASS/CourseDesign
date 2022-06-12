using CourseDesign.Common.Classes;
using CourseDesign.Common.Modules;
using CourseDesign.Context;
using CourseDesign.Extensions;
using CourseDesign.Services;
using CourseDesign.Services.API.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using static CourseDesign.Context.LoginUserContext;
using System.Collections.ObjectModel;
using Prism.Ioc;

namespace CourseDesign.ViewModels
{
    public class MainWindowViewModel : BindableBase, ILoginedConfigureService
    {
        #region 字段
        private readonly IContainerProvider containerProvider; // 主容器
        private readonly IRegionManager regionManager; // 区域控制器
        private IRegionNavigationJournal journal; // 区域导航日志
        // API服务
        private readonly IImagePlanService ImageService;
        private readonly ITextPlanService TextService;
        private readonly ITDollService TDollService;
        // 属性字段
        private UserClass user;
        private int selectIndex;                        // 所选择的主菜单哪一项
        // 导航栏部分
        private ObservableCollection<MenuBar> menuBars; // 主菜单列表
        #endregion

        #region 属性
        public int SelectIndex { get { return selectIndex; } set { selectIndex = value; RaisePropertyChanged(); } }
        public UserClass User { get { return user; } set { user = value; RaisePropertyChanged(); } }
        public ObservableCollection<MenuBar> MenuBars { get { return menuBars; } set { menuBars = value; } }
        // 命令部分
        public DelegateCommand<MenuBar> NavigationCommand { get; private set; } // 从UI层传递MenuBars到这个导航命令
        public DelegateCommand GoBackCommand { get; private set; } // 后退命令
        public DelegateCommand GoHomeCommand { get; private set; } // 返回主页命令
        public DelegateCommand LogoutCommand { get; private set; } // 退出登录命令
        #endregion

        #region 初始化
        /// <summary>
        /// MainWindow VM 的构造函数，登陆前调用：
        /// 完成服务、命令接口、全局上下文（人形数据）的初始化
        /// <para>注：一部分数据要等登陆后调用ConfigureForUser才加载</para>
        /// </summary>
        /// <param name="regionManager"> 区域管理器 </param>
        public MainWindowViewModel(IContainerProvider containerProvider, IRegionManager regionManager, ITextPlanService textPlanService, IImagePlanService imagePlanService, ITDollService tDollService)
        {
            this.containerProvider = containerProvider;
            this.regionManager = regionManager;
            TextService = textPlanService;
            ImageService = imagePlanService;
            TDollService = tDollService;
            // 命令接口实现
            NavigationCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
            GoHomeCommand = new DelegateCommand(GoIndex); // 主页定为Index页面，所以更名为GoIndex
            LogoutCommand = new DelegateCommand(() => App.Logout(containerProvider));
            // 创建全局静态上下文 - TDolls人形数据
            _ = new TDollsContext(tDollService); // 创建人形上下文
        }

        /// <summary>
        /// 主应用的默认配置，在登陆后才调用！
        /// </summary>
        public void ConfigureForUser(UserClass user)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            CreateMenuBars(); //创建菜单栏
            _ = new LoginUserContext(user.UserID, ImageService, TextService, TDollService); // 创建用户上下文
            User = user;
            GoIndex();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 返回上一页的实现方法
        /// </summary>
        private void GoBack()
        {
            if (journal != null && journal.CanGoBack)
                journal.GoBack();
            UpdateSelectIndex();
        }

        /// <summary>
        /// 返回Index页的实现方法
        /// </summary>
        private void GoIndex()
        {
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView", back => { journal = back.Context.NavigationService.Journal; });
            UpdateSelectIndex();
        }

        /// <summary>
        /// 用于驱动页面的切换的实现方法
        /// </summary>
        /// <param name="obj">当注册的导航MenuBar响应后，自动调用该方法，并作为参数obj</param>
        private void Navigate(MenuBar obj)
        {
            if (obj != null && !string.IsNullOrWhiteSpace(obj.Title))
                regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back => { journal = back.Context.NavigationService.Journal; });
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 在进行上一页和返回主页操作后，用来更新NavagationBar（菜单栏导航卡）中所选择的导航项的index，从而更新所选择的项。
        /// 否则操作后导航项不会更新
        /// </summary>
        private void UpdateSelectIndex()
        {
            int index = 0;
            foreach (MenuBar nowMenuBar in MenuBars)
                if (nowMenuBar.NameSpace == journal.CurrentEntry.Uri.ToString()) // journal中这个代表当前页面的uri（前面导航传进来的那个），因此可以遍历然后找到index
                    SelectIndex = index;
                else
                    index++;
        }

        /// <summary>
        /// 创建主菜单列表
        /// <para>注意：Add的顺序需要与主菜单列表的一致！否则在上一页、返回主页时，NavigationBar的更新会混乱</para>
        /// </summary>
        public void CreateMenuBars()
        {
            MenuBars.Add(new MenuBar("Home", "首页", "IndexView"));
            MenuBars.Add(new MenuBar("CheckboxMultipleMarkedCircleOutline", "计划列表", "PlanView"));
            MenuBars.Add(new MenuBar("BadgeAccount", "图鉴", "ListView"));
            MenuBars.Add(new MenuBar("Cog", "设置", "SettingView"));
        }
        #endregion
    }
}
