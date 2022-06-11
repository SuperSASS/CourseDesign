using CourseDesign.Common.Classes;
using CourseDesign.Common.Modules;
using CourseDesign.Context;
using CourseDesign.Extensions;
using CourseDesign.Services;
using CourseDesign.Services.API.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace CourseDesign.ViewModels
{
    public class MainWindowViewModel : BindableBase, IConfigureService
    {
        private readonly IRegionManager regionManager; // 区域控制器
        private IRegionNavigationJournal journal; // 区域导航日志

        // 导航栏部分
        private ObservableCollection<MenuBar> menuBars; // 主菜单列表
        private int selectIndex;                        // 所选择的主菜单哪一项
        public ObservableCollection<MenuBar> MenuBars { get { return menuBars; } set { menuBars = value; } }
        public int SelectIndex { get { return selectIndex; } set { selectIndex = value; RaisePropertyChanged(); } }

        // 命令部分
        public DelegateCommand<MenuBar> NavigationCommand { get; private set; } // 从UI层传递MenuBars到这个导航命令
        public DelegateCommand GoBackCommand { get; private set; } // 后退命令
        public DelegateCommand GoHomeCommand { get; private set; } // 返回主页命令

        /// <summary>
        /// MainWindow VM 的构造函数：
        /// <para>完成菜单栏、导航命令、后退命令、返回主页命令的初始化</para>
        /// </summary>
        /// <param name="regionManager"> 区域管理器 </param>
        public MainWindowViewModel(IRegionManager regionManager,ITextPlanService textPlanService, IImagePlanService imagePlanService,  ITDollService tDollService)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            this.regionManager = regionManager;
            // 命令接口实现
            NavigationCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
            GoHomeCommand = new DelegateCommand(GoIndex); // 主页定为Index页面，所以更名为GoIndex
            // 创建全局静态上下文
            _ = new LoginUserContext(1, imagePlanService, textPlanService, tDollService); // 创建用户上下文
            _ = new TDollsContext(tDollService); // 创建人形上下文
        }

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

        /// <summary>
        /// 创建主菜单列表
        /// <para>注意：Add的顺序需要与主菜单列表的一致！否则在上一页、返回主页时，NavigationBar的更新会混乱</para>
        /// </summary>
        void CreateMenuBars()
        {
            MenuBars.Add(new MenuBar("Home", "首页", "IndexView"));
            MenuBars.Add(new MenuBar("CheckboxMultipleMarkedCircleOutline", "计划列表", "PlanView"));
            MenuBars.Add(new MenuBar("BadgeAccount", "图鉴", "ListView"));
            MenuBars.Add(new MenuBar("Cog", "设置", "SettingView"));
        }

        /// <summary>
        /// 主窗口的默认配置
        /// </summary>
        public void Configure()
        {
            CreateMenuBars(); //创建菜单栏【注：由于Congigure在构造函数之后，构造函数里完成了对SelectionChanged的事件触发器构建，
                              //此时再创建菜单栏会触发该触发器，导致换到默认的SelectedIndex=0的页面，即第一个MenuBar首页。
        }
    }
}
