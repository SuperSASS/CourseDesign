using CourseDesign.Command.Modules;
using CourseDesign.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;

// TODO: 注意：在按按钮导航时，主菜单的导航栏不会被切换，需要fix
namespace CourseDesign.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private ObservableCollection<MeauBar> meauBars; // 主菜单列表
        public ObservableCollection<MeauBar> MeauBars { get { return meauBars; } set { meauBars = value; } }

        // 命令部分
        public DelegateCommand<MeauBar> NavigationCommand { get; private set; } // 从UI层传递MeauBars到这个导航命令
        public DelegateCommand GoBackCommand { get; private set; } // 后退命令
        public DelegateCommand GoHomeCommand { get; private set; } // 返回主页命令

        private readonly IRegionManager regionManager; // 区域控制器
        private IRegionNavigationJournal journal; // 区域导航日志

        /// <summary>
        /// MainWindow VM 的构造函数：
        /// <para>完成菜单栏、导航命令、后退命令、返回主页命令的初始化</para>
        /// </summary>
        /// <param name="reigionManager"> 区域管理器 </param>
        public MainWindowViewModel(IRegionManager reigionManager)
        {
            MeauBars = new ObservableCollection<MeauBar>();
            CreateMeauBars();
            NavigationCommand = new DelegateCommand<MeauBar>(Navigate);
            this.regionManager = reigionManager;

            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                    journal.GoBack();
            });
            GoHomeCommand = new DelegateCommand(() =>
            {
                reigionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView", back =>
                {
                    journal = back.Context.NavigationService.Journal;
                });
            });
        }

        /// <summary>
        /// 用于驱动页面的切换的实现方法
        /// </summary>
        /// <param name="obj">当注册的导航MeauBar响应后，自动调用该方法，并作为参数obj</param>
        private void Navigate(MeauBar obj)
        {
            if (obj != null && !string.IsNullOrWhiteSpace(obj.Title))
                regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
                {
                    journal = back.Context.NavigationService.Journal;
                });
        }
        

        /// <summary>
        /// 创建主菜单列表
        /// </summary>
        void CreateMeauBars()
        {
            MeauBars.Add(new MeauBar("Home", "首页", "IndexView"));
            MeauBars.Add(new MeauBar("CheckboxMultipleMarkedCircleOutline", "任务列表", "TaskView"));
            MeauBars.Add(new MeauBar("BadgeAccount", "图鉴", "ListView"));
            MeauBars.Add(new MeauBar("Cog", "设置", "SettingView"));
        }
    }
}
