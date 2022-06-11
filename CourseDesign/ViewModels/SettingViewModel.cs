using CourseDesign.Common.Modules;
using CourseDesign.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace CourseDesign.ViewModels
{
    public class SettingViewModel : BindableBase
    {
        private ObservableCollection<MenuBar> settingBars;  // 设置页列表
        public ObservableCollection<MenuBar> SettingBars { get { return settingBars; } set { settingBars = value; RaisePropertyChanged(); } }

        // 命令部分
        public DelegateCommand<MenuBar> NavigationCommand { get; private set; } // 从UI层传递SettingBars到这个导航命令

        private readonly IRegionManager regionManager; // 区域控制器

        /// <summary>
        /// SettingView VM 的构造函数：
        /// <para>完成设置栏和设置区域的初始化</para>
        /// </summary>
        /// <param name="regionManager"> 区域管理器 </param>
        public SettingViewModel(IRegionManager regionManager)
        {
            SettingBars = new ObservableCollection<MenuBar>();
            CreateSettingBars();
            NavigationCommand = new DelegateCommand<MenuBar>(Navigate);
            this.regionManager = regionManager;
        }

        /// <summary>
        /// 用于驱动页面的切换的实现方法
        /// </summary>
        /// <param name="obj">当注册的导航menuBar响应后，自动调用该方法，并作为参数obj</param>
        /// 
        private void Navigate(MenuBar obj)
        {
            if (obj != null && !string.IsNullOrWhiteSpace(obj.NameSpace))
                regionManager.Regions[PrismManager.SettingViewRegionName].RequestNavigate(obj.NameSpace);
        }

        /// <summary>
        /// 创建设置页置页的列表
        /// </summary>
        private void CreateSettingBars()
        {
            SettingBars.Add(new MenuBar("Palette", "主题", "SkinSettingView"));
            SettingBars.Add(new MenuBar("WrenchOutline", "其他设置", "OtherSettingView"));
            SettingBars.Add(new MenuBar("MessageReplyTextOutline", "关于", "AboutSettingView"));
        }
    }
}
