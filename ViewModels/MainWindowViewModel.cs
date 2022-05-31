using CourseDesign.Command.Module;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace CourseDesign.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            MeauBars = new ObservableCollection<MeauBar>();
            CreateMeauBar();
        }

        private ObservableCollection<MeauBar> meauBars;
        public ObservableCollection<MeauBar> MeauBars
        {
            get { return meauBars; }
            set { meauBars = value; }
        }

        /// <summary>
        /// 创建主菜单列表
        /// </summary>
        void CreateMeauBar()
        {
            MeauBars.Add(new MeauBar("Home", "首页", "IndexView"));
            MeauBars.Add(new MeauBar("Target", "远征", "CombatView"));
            MeauBars.Add(new MeauBar("AccountGroup", "抽卡", "GachaView"));
            MeauBars.Add(new MeauBar("Codepen", "培养", "TrainView"));
            MeauBars.Add(new MeauBar("BadgeAccount", "图鉴", "listView"));
            MeauBars.Add(new MeauBar("Cog", "设置", "SettingView"));
        }
    }
}
