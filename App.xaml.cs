using CourseDesign.ViewModels;
using CourseDesign.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;

namespace CourseDesign
{
    /// <summary>
    /// App.xaml的逻辑接口：
    /// <para>用来启动主页<c>MainWindow</c>和注册各种依赖的后端。</para>
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 注册依赖 - 导航
            containerRegistry.RegisterForNavigation<CombatView>(); // 不带第二个类型，默认是<xxx, xxxModel>
            containerRegistry.RegisterForNavigation<GachaView>();
            containerRegistry.RegisterForNavigation<IndexView>();
            containerRegistry.RegisterForNavigation<ListView>();
            containerRegistry.RegisterForNavigation<SettingView>();
            containerRegistry.RegisterForNavigation<TrainView>();
        }
    }
}
