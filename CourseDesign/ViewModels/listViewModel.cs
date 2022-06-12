using CourseDesign.Common.Classes;
using CourseDesign.Common.Classes.Bases;
using CourseDesign.Common.Modules;
using CourseDesign.Context;
using CourseDesign.ViewModels.Bases;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using static CourseDesign.Common.Classes.TDollClass;

namespace CourseDesign.ViewModels
{
    public class ListViewModel : DialogNavigationViewModel
    {
        #region 字段
        private string searchNameText; // 搜索的名称
        private int searchRarityIndex; // 搜索的稀有度
        private int searchTypeIndex; // 搜索的人形种类
        private bool isNoResult; // 是否没有搜索结果

        private ObservableCollection<TDollList> tDollList;
        #endregion

        #region 属性
        public DelegateCommand<string> ExecCommand { get; private set; } // 总命令执行
        /// <summary>
        /// 搜索人形名称文本
        /// </summary>
        public string SearchNameText
        {
            get { return searchNameText; }
            set { searchNameText = value; RaisePropertyChanged(); SearchPlan(); }
        }
        /// <summary>
        /// 搜索的人形稀有度
        /// </summary>
        public int SearchRarityIndex
        {
            get { return searchRarityIndex; }
            set { searchRarityIndex = value;RaisePropertyChanged(); SearchPlan(); }
        }
        /// <summary>
        /// 搜索的人形种类
        /// </summary>
        public int SearchTypeIndex
        {
            get { return searchTypeIndex; }
            set { searchTypeIndex = value; RaisePropertyChanged(); SearchPlan(); }
        }
        /// <summary>
        /// 是否没有搜索结果
        /// </summary>
        public bool IsNoResult
        {
            get { return isNoResult; }
            set { isNoResult = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TDollList> TDollsShow
        {
            get { return tDollList; }
            set { tDollList = value; }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="containerProvider"></param>
        public ListViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            // 初始展示属性初始化
            TDollsShow = new ObservableCollection<TDollList>();
            // 命令的初始化
            ExecCommand = new DelegateCommand<string>(Exec);
        }

        /// <summary>
        /// 重写导航加载到该页面的方法，每次来到该页面都会执行一次
        /// </summary>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            SearchNameText = null;
            SearchRarityIndex = -1;
            SearchTypeIndex = -1;
            SearchPlan(); // 来到该页面时，默认重新读取所有的展示数据
            CheckPresentation();
        }

        #region View的方法
        /// <summary>
        /// 命令执行的总命令
        /// </summary>
        /// <param name="cmd">执行的命令</param>
        private void Exec(string cmd)
        {
            try
            {
                switch (cmd)
                {
                    case "查询": SearchPlan(); break;
                    default: throw new Exception("内部错误 - 有命令的参数写错啦！快叫程序员来修……");
                }
            }
            catch (Exception ex)
            { ShowMessageDialog(ex.Message, "Main"); }
            finally { }
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 恒为真的<see cref="Func{TDollClass, bool}"/>类型的函数式，简化表达式用
        /// </summary>
        static readonly Func<TDollClass, bool> trueFunc = (x) => true;

        /// <summary>
        /// 查询该用户包含条件的计划，目前只支持文字类计划的标题包含搜索
        /// </summary>
        private void SearchPlan()
        {
            Func<TDollClass, bool> exp_Name, exp_Rarity, exp_Type;
            /* 生成有关名字筛选的表达式 */
            if (string.IsNullOrWhiteSpace(SearchNameText))
                exp_Name = trueFunc;
            else
                exp_Name = (x) => x.Name.Contains(SearchNameText);
            /* 生成有关稀有度筛选的表达式 */
            if (SearchRarityIndex == -1) // TODO: 1 - 验证没选的时候是不是-1
                exp_Rarity = trueFunc;
            else
                exp_Rarity = (x) => x.Rarity == (SearchRarityIndex + 2);
            /* 生成有关种类筛选的表达式 */
            if (SearchTypeIndex == -1) // TODO: 1 - 验证没选的时候是不是-1
                exp_Type = trueFunc;
            else
                exp_Type = (x) => x.Type == (TDollType)(SearchTypeIndex);

            CreateShowPlans(exp_Name, exp_Rarity, exp_Type);
            CheckPresentation();
        }

        /// <summary>
        /// 根据状态和字段的筛选，生成用于展示的计划
        /// </summary>
        private void CreateShowPlans(Func<TDollClass, bool> exp_Name, Func<TDollClass, bool> exp_Rarity, Func<TDollClass, bool> exp_Type)
        {
            TDollsShow.Clear();
            foreach (TDollClass item in TDollsContext.AllTDolls)
                if (exp_Name(item) && exp_Rarity(item) && exp_Type(item))
                    TDollsShow.Add(new TDollList(item));
        }

        /// <summary>
        /// 更新是否没有搜索结果的Presentation提示页的状态
        /// </summary>
        private void CheckPresentation()
        {
            IsNoResult = TDollsShow.Count == 0;
        }
        #endregion
    }
}
