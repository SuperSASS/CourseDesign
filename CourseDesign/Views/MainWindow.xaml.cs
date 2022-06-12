using CourseDesign.Extensions;
using CourseDesign.Services.Dialog;
using CourseDesign.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using Prism.Events;
using System.Windows;

namespace CourseDesign.Views
{
    /// <summary>
    /// 起始页的基本页面实现逻辑【只实现先基本的如关闭、最小化之类的页面逻辑，有关数据绑定之类的则分离到了后端ViewModel中
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IEventAggregator aggregator, IDialogHostService dialogHostService)
        {
            InitializeComponent();

            aggregator.RegisterLoadingDialog(arg => // 注册Dialog会话 - 弹窗等待
            {
                DialogHost.IsOpen = arg.IsOpen;
                if (DialogHost.IsOpen)
                    DialogHost.DialogContent = new ProgressView(); // 弹窗等待条
            });
            aggregator.RegisterMessageDialog(arg => // 注册Dialog会话 - 提示消息
            {
                snack_bar_main.MessageQueue.Enqueue(arg.Message); // 将传来的MessageModel的Message压入消息队列
            }, "Main");

            btn_min.Click += (s, e) => { this.WindowState = WindowState.Minimized; }; // 标题栏 - 窗口最小化按钮
            btn_max.Click += (s, e) => // 标题栏 - 窗口最大化按钮
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };
            btn_close.Click += async (s, e) =>
            {
                var dialogResult = await dialogHostService.ShowQueryDialog("退出系统", "确认要退出系统吗？");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.Yes) return;
                this.Close();
            }; // 标题栏 - 窗口关闭按钮

            titleBar.MouseLeftButtonDown += (s, e) => // 标题栏 - 窗口拖动
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
            titleBar.MouseDoubleClick += (s, e) => // 标题栏 - 窗口双击最大化
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };
            navigationBars.SelectionChanged += (s, e) => // 导航栏 - 更新左侧横条
            {
                menu_bars.IsLeftDrawerOpen = false;
            };
        }
    }
}
