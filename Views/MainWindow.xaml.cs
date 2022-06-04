using CourseDesign.Command.Modules;
using System.Windows;
using System.Windows.Controls;

namespace CourseDesign.Views
{
    /// <summary>
    /// 起始页的基本页面实现逻辑【只实现先基本的如关闭、最小化之类的页面逻辑，有关数据绑定之类的则分离到了后端ViewModel中
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            navigationBars.SelectedIndex = 3;

            btn_min.Click += (s, e) => { this.WindowState = WindowState.Minimized; }; // 菜单栏 - 窗口最小化按钮
            btn_max.Click += (s, e) => // 菜单栏 - 窗口最大化按钮
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };
            btn_close.Click += (s, e) => { this.Close(); }; // 菜单栏 - 窗口关闭按钮

            titleBar.MouseLeftButtonDown += (s, e) => // 菜单栏 - 窗口拖动
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
            titleBar.MouseDoubleClick += (s, e) => // 菜单栏 - 窗口双击最大化
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };
            navigationBars.SelectionChanged += (s, e) => // 导航栏 - 更新左侧横条
            {
                meauBars.IsLeftDrawerOpen = false;
            };
        }
    }
}
