using CourseDesign.Services;
using CourseDesign.Services.Interfaces;
using CourseDesign.ViewModels;
using CourseDesign.ViewModels.Settings;
using CourseDesign.Views;
using CourseDesign.Views.Settings;
using DryIoc;
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
            // 注册依赖 - 导航：菜单栏
            containerRegistry.RegisterForNavigation<IndexView>();
            containerRegistry.RegisterForNavigation<ListView>();
            containerRegistry.RegisterForNavigation<SettingView>();
            containerRegistry.RegisterForNavigation<PlanView>();
            // 注册依赖 - 导航：设置页
            containerRegistry.RegisterForNavigation<SkinSettingView, SkinSettingViewModel>(); // 由于不在ViewModels目录中，所以要写上第二类型
            containerRegistry.RegisterForNavigation<OtherSettingView, OtherSettingViewModel>(); // 由于不在ViewModels目录中，所以要写上第二类型
            containerRegistry.RegisterForNavigation<AboutSettingView, AboutSettingViewModel>(); // 由于不在ViewModels目录中，所以要写上第二类型
            // 注册依赖 - API相关【不是很理解，先写下来
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl")); // 先给构造函数设置一个默认名称
            containerRegistry.GetContainer().RegisterInstance(@"http://localhost:2333/", serviceKey: "webUrl"); // 注册服务地址
            containerRegistry.Register<IImagePlanService, ImagePlanService>(); // 注册ImagePlan的服务（接口，实现）

        }
    }
}
