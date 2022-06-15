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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
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
        // 内部字段
        private bool isNavigate_SNT; // 这些字段防止第一次Navigate到该页面时，重复调用SearchPlan
        private bool isNavigate_SRI;
        private bool isNavigate_STI;
        CancellationTokenSource cancelTokenSource;
        #endregion

        #region 属性
        public DelegateCommand<string> ExecCommand { get; private set; } // 总命令执行
        // 绑定属性
        /// <summary>
        /// 搜索人形名称文本（注意：会自动进行搜索）
        /// </summary>
        public string SearchNameText
        {
            get { return searchNameText; }
            set { searchNameText = value; RaisePropertyChanged(); if (isNavigate_SNT) isNavigate_SNT = false; else SearchPlan(); }
        }
        /// <summary>
        /// 搜索的人形稀有度（注意：会自动进行搜索）
        /// </summary>
        public int SearchRarityIndex
        {
            get { return searchRarityIndex; }
            set { searchRarityIndex = value; RaisePropertyChanged(); if (isNavigate_SRI) isNavigate_SRI = false; else SearchPlan(); }
        }
        /// <summary>
        /// 搜索的人形种类（注意：会自动进行搜索）
        /// </summary>
        public int SearchTypeIndex
        {
            get { return searchTypeIndex; }
            set { searchTypeIndex = value; RaisePropertyChanged(); if (isNavigate_STI) isNavigate_STI = false; else SearchPlan(); }
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
            set { tDollList = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 构造函数，只在刚导航到这页面执行一次，后面再导航到该页面不再执行
        /// </summary>
        public ListViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            // 初始展示属性初始化
            TDollsShow = new ObservableCollection<TDollList>();
            // 命令的初始化
            ExecCommand = new DelegateCommand<string>(Exec);
            cancelTokenSource = new();
        }

        /// <summary>
        /// 重写导航加载到该页面的方法，每次来到该页面都会执行一次
        /// </summary>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            isNavigate_SNT = isNavigate_SRI = isNavigate_STI = true; // 将这个赋为true，防止因属性的set而反复出发SearchPlan
            SearchNameText = null;
            SearchRarityIndex = -1;
            SearchTypeIndex = -1;
            SearchPlan();
        }
        #endregion

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
        /// 按用户筛选条件，生成表达式，供CreateShowPlans使用，并检查是否无筛选结果
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
            if (SearchRarityIndex == -1)
                exp_Rarity = trueFunc;
            else
                exp_Rarity = (x) => x.Rarity == (SearchRarityIndex + 2);
            /* 生成有关种类筛选的表达式 */
            if (SearchTypeIndex == -1)
                exp_Type = trueFunc;
            else
                exp_Type = (x) => x.Type == (TDollType)(SearchTypeIndex);
            cancelTokenSource.Cancel();
            cancelTokenSource = new();
            CreateShowPlans(exp_Name, exp_Rarity, exp_Type, cancelTokenSource.Token);
            //CheckPresentation(); // 由于CreateShowPlans内部异步，没搜索到结果就会到这来，所以不能在这check
        }

        /// <summary>
        /// 根据筛选条件形成的表达式，生成用于展示的人形数据（里面采用了InvokeAsync搭配await Task.Delay，使得数据缓缓载入……
        /// </summary>
        private void CreateShowPlans(Func<TDollClass, bool> exp_Name, Func<TDollClass, bool> exp_Rarity, Func<TDollClass, bool> exp_Type, CancellationToken cancellationToken)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(async () =>
            {
                TDollsShow.Clear();
                foreach (TDollClass item in TDollsContext.AllTDolls)
                {
                    if (!cancellationToken.IsCancellationRequested && exp_Name(item) && exp_Rarity(item) && exp_Type(item)) // 找到满足条件的，通过UI线程加入TDollsShow
                    {
                        TDollsShow.Add(new TDollList(item));
                        CheckPresentation(); // 这里应该有个check，添加了一个就要让Presentation马上消失
                        await Task.Delay(50); // 这个delay要放在Add的语句块里，不然遍历时不符合条件没添加也会delay，很不正常
                    }
                    if (cancellationToken.IsCancellationRequested)
                        break;
                };
                CheckPresentation(); // 该在这check，等全部遍历完了都没找到
            });
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
