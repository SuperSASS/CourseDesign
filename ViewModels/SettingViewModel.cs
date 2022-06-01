using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseDesign.Command.Modules;
using CourseDesign.Extensions;
using Prism.Commands;
using Prism.Regions;

namespace CourseDesign.ViewModels
{
    internal class SettingViewModel
    {
        private ObservableCollection<MeauBar> settingBars;
        public ObservableCollection<MeauBar> SettingBars { get { return settingBars; } set { settingBars = value; } }
        public DelegateCommand<MeauBar> NavigationCommand { get; private set; }

        private readonly IRegionManager regionManager;

        /// <summary>
        /// SettingView VM 的构造函数：
        /// <para>完成设置栏和设置区域的初始化</para>
        /// </summary>
        /// <param name="regionManager"> 区域管理器 </param>
        public SettingViewModel(IRegionManager regionManager)
        {
            SettingBars = new ObservableCollection<MeauBar>();
            CreateSettingBars();
            NavigationCommand = new DelegateCommand<MeauBar>(Navigate);
            this.regionManager = regionManager;
        }

        private void Navigate(MeauBar obj)
        {
            if (obj != null && !string.IsNullOrWhiteSpace(obj.NameSpace))
                regionManager.Regions[PrismManager.SettingViewRegionName].RequestNavigate(obj.NameSpace);
        }

        /// <summary>
        /// 创建设置页置页的列表
        /// </summary>
        private void CreateSettingBars()
        {
            SettingBars.Add(new MeauBar("Home", "主题", "SkinSettingView"));
            SettingBars.Add(new MeauBar("Home", "其他设置", "OtherSettingView"));
            SettingBars.Add(new MeauBar("Home", "关于", "AboutView"));
        }
    }
}
