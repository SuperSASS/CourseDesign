using CourseDesign.Common.Classes;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using CourseDesign.Context;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Windows.Input;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using CourseDesign.Services;
using System.Linq.Expressions;
using static CourseDesign.Context.LoginUserContext;
using CourseDesign.Services.API.Interfaces;
using CourseDesign.Services.Dialog;
using CourseDesign.Extensions;
using CourseDesign.Common.Classes.Bases;
using CourseDesign.ViewModels.Bases;
using CourseDesign.Common.Modules;

namespace CourseDesign.ViewModels
{
    public class PlanViewModel : DialogNavigationViewModel
    {
        #region 字段
        // API服务
        private readonly IImagePlanService ImageService;
        private readonly ITextPlanService TextService;
        private readonly ITDollService TDollService;
        // 对话服务
        private readonly IDialogHostService DialogService;
        // 属性内部字段
        private ObservableCollection<PlanBase> plansShow; // 要展示的计划表
        private ObservableCollection<AddImagePlanList> addImagePlanSource; // 能增加的图片类计划源
        private string rightEditorTitle; // 右侧编辑栏的标题（主要用来区别是新增还是修改）
        private string rightEditorButton; // 右侧编辑栏的按钮
        private bool isRightTextEditorOpen; // 右侧编辑窗弹出情况 - 文本编辑页
        private bool isRightImageEditorOpen; // 右侧编辑窗弹出情况 - 图像编辑页
        private string searchContentText; // 所搜索的文本
        private int searchFieldIndex; // 筛选字段的选项Index，只用来初始化用
        private int searchStatusIndex; // 选择计划状态的选项Index【0 - 已完成； 1 - 未完成（null？ - 全部）
        private PlanBase currentEditPlan; // 当前在编辑栏中编辑的对象，双向绑定
        private bool isNothing; // 是否未添加计划
        private bool isAllComplete; // 是否已完成所有计划（不包含未添加计划情况）
        private bool isNoResult; // 该搜索条件下是否没有数据
        private bool isNoComplete; // 是否没有完成的计划
        // 本地字段
        private bool isAddOrModify; // 0 为 Add, 1 为 Modify
        #endregion

        #region 属性
        // 命令绑定
        public DelegateCommand<string> EditOfAddPlanCommand { get; private set; } // 增加命令（不知道这种能不能合并到Exec中，待修改）
        public DelegateCommand<PlanBase> EditOfModifyPlanCommand { get; private set; } // 修改命令
        public DelegateCommand<PlanBase> DeletePlanCommand { get; private set; } // 删除命令
        public DelegateCommand<TextPlanClass> ChangeCompleteForTextPlanCommand { get; private set; } // 切换文字状态命令
        public DelegateCommand<ImagePlanClass> CompleteForImagePlanCommand { get; private set; } // 完成图片计划命令

        public DelegateCommand<string> ExecCommand { get; private set; } // 执行无参数命令（有参数的因为CommandParameter要绑定参数，无法绑定命令，只能用另一种）
        public DelegateCommand SearchPlanCommand { get; private set; } // 查找命令
        public DelegateCommand UpdatePlanCommand { get; private set; } // 上传更新命令

        // 外部访问属性
        /// <summary>
        /// 所展示的计划列表
        /// </summary>
        public ObservableCollection<PlanBase> PlansShow { get { return plansShow; } private set { plansShow = value; RaisePropertyChanged(); } } // 当展示计划更改时，会检查所有的Persentation属性
        /// <summary>
        /// 在添加人形获取计划中，所能展示的人形列表
        /// </summary>
        public ObservableCollection<AddImagePlanList> AddImagePlanSource { get { return addImagePlanSource; } private set { addImagePlanSource = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑栏的标题
        /// </summary>
        public string RightEditorTitle { get { return rightEditorTitle; } private set { rightEditorTitle = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑栏的按钮
        /// </summary>
        public string RightEditorButton { get { return rightEditorButton; } private set { rightEditorButton = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑窗弹出情况，0为不弹出，1为弹出文本类编辑栏，2为弹出图片类编辑栏
        /// </summary>
        public bool IsRightTextEditorOpen { get { return isRightTextEditorOpen; } set { isRightTextEditorOpen = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑窗弹出情况，0为不弹出，1为弹出文本类编辑栏，2为弹出图片类编辑栏
        /// </summary>
        public bool IsRightImageEditorOpen { get { return isRightImageEditorOpen; } set { isRightImageEditorOpen = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 搜索文本，单向到源绑定
        /// </summary>
        public string SearchContentText { get { return searchContentText; } set { searchContentText = value; RaisePropertyChanged(); } }

        private bool isFirstCreate_SFT = true; // 加这个临时字段，防止第一次加载时，计划数据被重复读取（会调用SearchFiledText、SearchStatus和初始化各一次，又因为异步可能不会等全部清楚后再添加
        private bool isFirstCreate_SS = true; // 加这个临时字段，防止第一次加载时，计划数据被重复读取（会调用SearchFiledText、SearchStatus和初始化各一次，又因为异步可能不会等全部清楚后再添加

        /// <summary>
        /// 搜索字段，单项绑定到源
        /// </summary>
        public int SearchFieldIndex
        {
            get { return searchFieldIndex; }
            set { searchFieldIndex = value; RaisePropertyChanged(); if (isFirstCreate_SFT) isFirstCreate_SFT = false; else SearchPlan(); }
        }
        /// <summary>
        /// 筛选的状态，单向绑定到源
        /// </summary>
        public int SearchStatusIndex
        {
            get { return searchStatusIndex; }
            set { searchStatusIndex = value; RaisePropertyChanged(); if (isFirstCreate_SS) isFirstCreate_SS = false; else SearchPlan(); }
        }
        /// <summary>
        /// 所选择的将要进行修改的计划，双向绑定，展示到文本栏同步更改
        /// </summary>
        public PlanBase CurrentEditPlan { get { return currentEditPlan; } set { currentEditPlan = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 该用户是否没有计划
        /// </summary>
        public bool IsNothing { get { return isNothing; } set { isNothing = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 该用户是否已完成所有计划（不包含没有计划情况）
        /// </summary>
        public bool IsAllComplete { get { return isAllComplete; } set { isAllComplete = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 搜索条件没有数据
        /// </summary>
        public bool IsNoResult { get { return isNoResult; } set { isNoResult = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 是否没有完成的计划
        /// </summary>
        public bool IsNoComplete { get { return isNoComplete; } set { isNoComplete = value; RaisePropertyChanged(); } }
        #endregion

        /// <summary>
        /// Init - 构造函数，只执行一次
        /// </summary>
        /// <param name="imageService">图片类计划的服务</param>
        /// <param name="textService">文本类计划的服务</param>
        /// <param name="containerProvider">该页面容器</param>
        public PlanViewModel(IImagePlanService imageService, ITextPlanService textService, ITDollService tDollService, IContainerProvider containerProvider) : base(containerProvider)
        {
            // 内部服务初始化
            ImageService = imageService;
            TextService = textService;
            TDollService = tDollService;
            DialogService = containerProvider.Resolve<IDialogHostService>();
            // 部分初始展示属性初始化
            plansShow = new ObservableCollection<PlanBase>();
            AddImagePlanSource = new ObservableCollection<AddImagePlanList>();
            // 各种命令的初始化
            EditOfAddPlanCommand = new DelegateCommand<string>(EditOfAddPlan);
            EditOfModifyPlanCommand = new DelegateCommand<PlanBase>(EditOfModifyPlan);
            DeletePlanCommand = new DelegateCommand<PlanBase>(DeletePlan);
            ChangeCompleteForTextPlanCommand = new DelegateCommand<TextPlanClass>(ChangeCompleteForTextPlan);
            CompleteForImagePlanCommand = new DelegateCommand<ImagePlanClass>(CompleteForImagePlan);
            ExecCommand = new DelegateCommand<string>(Exec);
            SearchPlanCommand = new DelegateCommand(SearchPlan);
            UpdatePlanCommand = new DelegateCommand(UpdatePlan);
        }

        #region 方法
        /// <summary>
        /// 重写导航加载到该页面的方法，每次来到该页面都会执行一次
        /// </summary>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            IsRightTextEditorOpen = false;
            IsRightImageEditorOpen = false;
            SearchFieldIndex = -1;
            SearchStatusIndex = 1; // 每次来到页面：默认跳回选择“未完成”状态筛选
            SearchContentText = null;
            SearchPlan(); // 来到该页面时，默认重新读取用户所有的数据（用SearchPlan哦，因为默认读未完成计划
            CheckPresentation();
        }

        // 已验证√
        /// <summary>
        /// 增加计划
        /// </summary>
        /// <param name="type">传的参数，用"Text"表示增加的文本类，"Image"表示增加的图片类</param>
        private void EditOfAddPlan(string type)
        {
            isAddOrModify = true; // 记录操作是增加

            if (type.Equals("Text")) // 选中的文本类消息
            {
                IsRightTextEditorOpen = true; // 窗口为“文本编辑状态”
                RightEditorTitle = "常规计划添加";
                RightEditorButton = "确认添加";
                CurrentEditPlan = new TextPlanClass(0, false, null, null);
            }
            else // 选中的图片类消息
            {
                CreateImagePlanSource(); // 生成用于图片编辑页的数据源
                IsRightImageEditorOpen = true; // 窗口为“图片编辑状态”
                RightEditorTitle = "人形计划添加";
                RightEditorButton = "确认添加";
                //CurrentEditPlan = new ImagePlanClass(0, false, -1); // 对于图片类构造方式不一样，因此不用生成
            }
        }

        // 已验证√
        /// <summary>
        /// 修改计划（会赋值给右侧编辑栏的用于相应属性SelectUpdatePlan，用于更改）
        /// </summary>
        /// <param name="obj">修改计划的基类</param>
        /// TODO: 2 - 在修改完右侧图片编辑栏的显示方法后，需要被修改
        private void EditOfModifyPlan(PlanBase obj)
        {
            isAddOrModify = false; // 操作是修改

            if (obj is TextPlanClass) // 选中的文本类消息
            {
                IsRightTextEditorOpen = true; // 窗口为“文本编辑状态”
                RightEditorTitle = "常规计划修改";
                RightEditorButton = "确认修改";
                TextPlanClass t = (TextPlanClass)obj;
                CurrentEditPlan = new TextPlanClass(t.ID, t.Status, t.Title, t.Content);
            }
            //else // 选中的图片类消息【不再让图片计划修改【因为重添加难度并不高
            //{
            //    CreateImagePlanSource();
            //    IsRightImageEditorOpen = true; // 窗口为“图片编辑状态”
            //    RightEditorTitle = "人形计划修改";
            //    RightEditorButton = "确认修改";
            //    ImagePlanClass t = (ImagePlanClass)obj;
            //    CurrentEditPlan = new ImagePlanClass(t.ID, t.Status, t.TDoll_ID);
            //}
        }

        // 已验证√
        /// <summary>
        /// 删除计划
        /// </summary>
        /// <param name="deletePlan">所删除计划的基类</param>
        private async void DeletePlan(PlanBase deletePlan)
        {
            try
            {
                var dialogResult = await DialogService.ShowQueryDialog("确认删除计划", $"确认要删除该计划吗？");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.Yes) // 没有确认
                    return;

                ShowLoadingDialog(true);

                var deleteResponse = deletePlan.Type == PlanBase.PlanType.Text ? await TextService.Delete(deletePlan.ID) : await ImageService.Delete(deletePlan.ID);
                if (deleteResponse.Status != APIStatusCode.Success)
                    throw new Exception("内部错误(API) - 删除出错啦，肯定不是服务器的问题qwq！……");
                { UserPlans.Remove(deletePlan); PlansShow.Remove(deletePlan); UserPlansComplete--; } // 删除的时候完成计划也要--
            }
            catch (Exception ex)
            {
                ShowMessageDialog(ex.Message, "Main");
            }
            finally
            {
                CheckPresentation();
                ShowLoadingDialog(false);
            }
        }

        // 已验证√
        /// <summary>
        /// 对文字类计划，切换任务完成状态的命令
        /// </summary>
        /// <param name="textPlan">所完成的任务，传的为基类</param>
        private async void ChangeCompleteForTextPlan(TextPlanClass textPlan)
        {
            APIStatusCode APIResponseStatus;
            try
            {
                ShowLoadingDialog(true);
                TextPlanClass plan = textPlan;
                plan.Status = textPlan.Status;
                APIResponseStatus = (await TextService.Update(plan.ConvertDTO(plan, LoginUserID))).Status;
                if (APIResponseStatus != APIStatusCode.Success)
                    throw new Exception("内部错误(API) - 诶，好像改不了这个任务的状态，待会再试试呢【……");
                GetPlan(textPlan.ID).Status = plan.Status; // 将本地的状态也更改
                if (plan.Status == false) // 证明从完成改到未完成
                    UserPlansComplete--;
                else // 从未完成改到完成
                    UserPlansComplete++;
                SearchPlan(); // 重新生成显示内容【这边应该执行SearchPlan，因为可能此时带有一些搜索条件
            }
            catch (Exception ex)
            {
                ShowMessageDialog(ex.Message, "Main");
            }
            finally
            {
                ShowLoadingDialog(false);
            }
        }

        // TODO: 1 - 当图片类计划完成后，自然要上传数据库表示得到了该人形
        /// <summary>
        /// 对图片类计划，只能完成任务的命令
        /// </summary>
        private async void CompleteForImagePlan(ImagePlanClass imagePlan)
        {
            APIStatusCode APIResponseStatus;
            try
            {
                ShowLoadingDialog(true);
                ImagePlanClass plan = imagePlan;
                plan.Status = true; // 对于图片类计划，只能完成，所以要单独提出来orz……
                APIResponseStatus = (await ImageService.Update(plan.ConvertDTO(plan, LoginUserID))).Status;
                if (APIResponseStatus != APIStatusCode.Success)
                    throw new Exception("内部错误(API) - 诶，好像改不了这个任务的状态，待会再试试呢【……");
                GetPlan(imagePlan.ID).Status = plan.Status; // 将本地的状态也更改
                // 上传获得的人形
                APIResponseStatus = (await TDollService.AddUserObtain(new TDollObtainDTO() { UserID = LoginUserID, TDollID = (int)imagePlan.TDoll_ID })).Status;
                if (APIResponseStatus != APIStatusCode.Success)
                    throw new Exception("内部错误(API) - 用户那里添加不了获取的人形……");
                UserTDolls.Add(imagePlan.TDoll_ID); // 注意：这里要修改本地上下文
                UserPlansComplete++; // 完成计划数++
                SearchPlan(); // 重新生成显示内容
            }
            catch (Exception ex)
            {
                ShowMessageDialog(ex.Message, "Main");
            }
            finally
            {
                ShowLoadingDialog(false);
            }
        }

        // 已验证√
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
                    // case "新增": EditOfAddPlan(param); break;
                    // case "修改": EditOfModifyPlan((PlanBase)param); break;
                    // case "完成": CompletePlan(); break;
                    // case "删除": DeletePlan(); break;
                    case "查询": SearchPlan(); break;
                    case "上传": UpdatePlan(); break;
                    default: throw new Exception("内部错误 - 有命令的参数写错啦！快叫程序员来修……");
                }
            }
            catch (Exception ex)
            { ShowMessageDialog(ex.Message,"Main"); }
            finally { }
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 恒为真的<see cref="Func{PlanBase, bool}"/>类型的函数式，简化表达式用
        /// </summary>
        static readonly Func<PlanBase, bool> trueFunc = (x) => true;

        /// <summary>
        /// 查询该用户包含条件的计划，目前只支持文字类计划的标题包含搜索
        /// </summary>
        private void SearchPlan()
        {
            Func<PlanBase, bool> exp_Status;
            /* 生成有关状态筛选的表达式 */
            switch (SearchStatusIndex)
            {
                case 0: exp_Status = (x) => x.Status == true; break;
                case 1: exp_Status = (x) => x.Status == false; break;
                default: exp_Status = trueFunc; break;
            }
            /* 生成有关字段筛选的表达式 */
            Func<PlanBase, bool> exp_Field;
            if (string.IsNullOrWhiteSpace(SearchContentText) || SearchFieldIndex == -1)
                exp_Field = trueFunc;
            else
            {
                switch (SearchFieldIndex)
                {
                    case 0: exp_Field = (x) => x is TextPlanClass && (x as TextPlanClass).Title.Contains(SearchContentText); break;
                    case 1: exp_Field = (x) => x is TextPlanClass && (x as TextPlanClass).Content.Contains(SearchContentText); break;
                    default: exp_Field = trueFunc; break;
                }
            }
            CreateShowPlans(exp_Status, exp_Field);
            CheckPresentation();
        }

        // 已验证√
        /// <summary>
        /// 上传该用户所选择需要更新或新增的计划
        /// </summary>
        private async void UpdatePlan()
        {
            try
            {
                if (IsRightTextEditorOpen) // 打开的是文字编辑
                {
                    TextPlanClass textPlan = (TextPlanClass)CurrentEditPlan;
                    if (string.IsNullOrWhiteSpace(textPlan.Title) || string.IsNullOrWhiteSpace(textPlan.Content)) // 计划的标题或内容为空
                        throw new Exception("计划要写好标题和内容啦_(:зゝ∠)_……"); // 返回错误提示
                    ShowLoadingDialog(true);
                    APIResponse<TextPlanDTO> updateResponse = isAddOrModify ? await TextService.Add(textPlan.ConvertDTO(textPlan, LoginUserID)) : await TextService.Update(textPlan.ConvertDTO(textPlan, LoginUserID));
                    if (updateResponse.Status != APIStatusCode.Success)
                        throw new Exception("内部错误(API) - " + updateResponse.Message);

                    if (isAddOrModify) // 代表新增
                    { UserPlans.Add(textPlan); PlansShow.Insert(0, textPlan); }
                    else
                    {
                        // 由于ObservableCollection没有FindIndex方法，所以只能手动模拟了……
                        int indexPlansShow, indexUserPlans = UserPlans.FindIndex((x) => x.ID == textPlan.ID);
                        for (indexPlansShow = 0; indexPlansShow <= PlansShow.Count; indexPlansShow++)
                            if (PlansShow[indexPlansShow] is TextPlanClass && PlansShow[indexPlansShow].ID == textPlan.ID) // 通过遍历ID找到所修改的数据
                                break;
                        if (indexUserPlans == -1 || indexPlansShow >= PlansShow.Count) // 遍历时出错
                            throw new Exception("内部错误 - 修改计划后，遍历的index无法找到对应的数据…");
                        UserPlans[indexUserPlans] = textPlan;
                        PlansShow[indexPlansShow] = textPlan;
                    }
                }
                else
                {
                    //if (imagePlan.TDoll_ID < 0 || imagePlan.TDoll_ID > TDollsContext.MaxTDoll_ID) // 人形计划的人形ID不满足范围【由于只展示人形列表的人形，不存在越界问题
                    //    throw new Exception("输入的战术人形ID不存在啦，请检查一下呢……"); // 返回错误提示
                    ShowLoadingDialog(true);
                    foreach (var item in AddImagePlanSource)
                        if (item.IsChecked && item.IsDefaultEnabled) // 被选了，并且默认是启用可选的，代表是用户选的——上传添加
                        {
                            ImagePlanClass imagePlan = new ImagePlanClass(0, false, item.TDoll.ID);
                            APIResponse<ImagePlanDTO> updateResponse = await ImageService.Add(imagePlan.ConvertDTO(imagePlan, LoginUserID)); // 图片类只能被添加
                            if (updateResponse.Status != APIStatusCode.Success) // 成功上传，添加到UserPlans和PlansShow
                                throw new Exception("内部错误(API) - 无法上传新建计划到服务器……");
                            imagePlan.ID = updateResponse.Result.ID;
                            UserPlans.Add(imagePlan);
                            PlansShow.Insert(0, imagePlan);
                        }
                }
            }
            catch (Exception ex)
            {
                ShowMessageDialog(ex.Message, "Main");
            }
            finally
            {
                CheckPresentation();
                ShowLoadingDialog(false);
                IsRightTextEditorOpen = false;
                IsRightImageEditorOpen = false; // 关闭编辑页
            }
        }   

        /// <summary>
        /// 根据状态和字段的筛选，生成用于展示的计划
        /// </summary>
        private void CreateShowPlans(Func<PlanBase, bool> exp_Status, Func<PlanBase, bool> exp_Field)
        {
            PlansShow.Clear();
            foreach (PlanBase item in UserPlans)
                if (exp_Status(item) && exp_Field(item))
                    PlansShow.Add(item);
        }

        /// <summary>
        /// 更新是否无计划或全部完成等Presentation提示页的状态
        /// </summary>
        private void CheckPresentation()
        {
            IsNothing = UserPlans.Count == 0;
            IsNoResult = !IsNothing && (!(string.IsNullOrWhiteSpace(SearchContentText) || SearchFieldIndex == -1) && PlansShow.Count == 0);
            IsAllComplete = !IsNothing && !IsNoResult && SearchStatusIndex == 1 && UserPlans.Count == UserPlansComplete;
            IsNoComplete = !IsNothing && !IsNoResult && SearchStatusIndex == 0 && UserPlansComplete == 0;
        }

        /// <summary>
        /// 生成用于在“添加人形获取计划”列表中展示的数据源
        /// </summary>
        private void CreateImagePlanSource()
        {
            AddImagePlanSource.Clear();
            foreach (TDollClass item in TDollsContext.AllTDolls)
                AddImagePlanSource.Add(new AddImagePlanList(item, UserTDolls.Contains(item.ID))); // 先筛选一遍用户没有的（并全部加到数据源中）
            foreach (PlanBase plan in UserPlans)
                if (plan is ImagePlanClass && plan.Status == false) // 再筛用户未完成图片计划里的
                {
                    int index = 0;
                    foreach (var item in AddImagePlanSource)
                    {
                        if (item.TDoll.ID == (plan as ImagePlanClass).TDoll_ID)
                            break;
                        index++;
                    }
                    AddImagePlanSource[index].IsChecked = true;
                    AddImagePlanSource[index].IsDefaultEnabled = false;
                }
        }
        #endregion
    }
}