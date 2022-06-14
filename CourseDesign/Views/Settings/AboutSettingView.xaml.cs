using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CourseDesign.Views.Settings
{
    /// <summary>
    /// AboutSettingView.xaml 的交互逻辑
    /// </summary>
    public partial class AboutSettingView : UserControl
    {
        public AboutSettingView()
        {
            InitializeComponent();
        }

        public void Hyperlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = ((Hyperlink)sender).NavigateUri.ToString(),
                UseShellExecute = true
            }) ;
        }
    }
}
