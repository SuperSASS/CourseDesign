using CourseDesign.Common.Classes;
using CourseDesign.Services;
using CourseDesign.Services.API;
using CourseDesign.Services.API.ClassServices;
using CourseDesign.Services.API.Interfaces;
using CourseDesign.Services.Dialog;
using CourseDesign.ViewModels;
using CourseDesign.ViewModels.Dialogs;
using CourseDesign.ViewModels.Settings;
using CourseDesign.Views;
using CourseDesign.Views.Dialogs;
using CourseDesign.Views.Settings;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Windows;

namespace CourseDesign
{
    /// <summary>
    /// App.xaml的逻辑接口：
    /// <para>用来启动主页<c>MainWindow</c>和注册各种依赖的后端。</para>
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// 第一个启动的首页(Shell)
        /// </summary>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>(); // 这里会调用MainWindowViewModel的构造函数，因此加载用户上下文不能放在构造函数
        }

        /// <summary>
        /// 重写初始化函数，使得可以加载MainWindow的默认配置（configureService.Configure()）
        /// </summary>
        protected override void OnInitialized()
        {
            var dialogService = Container.Resolve<IDialogService>();
            // 先展示登陆弹窗
            dialogService.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Current.Shutdown();
                    return;
                }
                var configureService = Current.MainWindow.DataContext as IMainWindowConfigureService;
                if (configureService != null)
                {
                    IDialogParameters returnParam = callback.Parameters;
                    configureService.ConfigureForUser(returnParam.GetValue<UserClass>("User")); // 登陆好后，将Dialog通过参数返回的UserID给初始化函数，再初始化MainWindow，故能加载用户上下文
                }
                base.OnInitialized(); // 这个是初始化MainWindow
            });
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
            containerRegistry.RegisterForNavigation<UserSettingView, UserSettingViewModel>();
            containerRegistry.RegisterForNavigation<AboutSettingView, AboutSettingViewModel>();
            // 注册依赖 - API相关
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl")); // 先给构造函数设置一个默认名称
            containerRegistry.GetContainer().RegisterInstance(@"http://localhost:2333/", serviceKey: "webUrl"); // 注册服务地址
            containerRegistry.Register<IImagePlanService, ImagePlanService>(); // 注册ImagePlan的服务
            containerRegistry.Register<ITextPlanService, TextPlanService>();   // 注册TextPlan的服务
            containerRegistry.Register<ITDollService, TDollService>();         // 注册TDoll的服务
            containerRegistry.Register<ILoginService, LoginService>();         // 注册Login的服务
            // 注册依赖 - Dialog弹窗服务
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            // 注册 - Prism的弹窗
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>(); // 登陆界面
            // 注册依赖 - 弹窗(Dialog)[由于不再使用Prism本身的，而是我们扩展的，所以改成容器注入
            containerRegistry.RegisterForNavigation<AddTextPlanView, AddTextPlanViewModel>();
            containerRegistry.RegisterForNavigation<QueryView, QueryViewModel>();
        }

        #region 外部全局方法
        /// <summary>
        /// 用户退出登录的方法
        /// </summary>
        public static void Logout(IContainerProvider containerProvider)
        {
            // 方法跟OnInitialized()类似
            Current.MainWindow.Hide(); // 隐藏主页面

            var dialogService = containerProvider.Resolve<IDialogService>();
            // 先展示登陆弹窗
            dialogService.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Current.Shutdown();
                    return;
                }
                var configureService = Current.MainWindow.DataContext as IMainWindowConfigureService;
                if (configureService != null)
                {
                    IDialogParameters returnParam = callback.Parameters;
                    configureService.ConfigureForUser(returnParam.GetValue<UserClass>("User")); // 登陆好后，将Dialog通过参数返回的UserID给初始化函数，再初始化MainWindow，故能加载用户上下文
                }
            });

            Current.MainWindow.Show(); // 重现主页面
        }
        
        public static void UpdateUser(UserClass user)
        {
            var configureService = Current.MainWindow.DataContext as IMainWindowConfigureService;
            configureService.UpdateForUser(user);
        }
        #endregion
    }
}
