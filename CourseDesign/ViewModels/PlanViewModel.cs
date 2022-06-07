using CourseDesign.Common.Classes;
using CourseDesign.Services.Interfaces;
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

namespace CourseDesign.ViewModels
{
    public class PlanViewModel : NavigationViewModel
    {
        #region 字段
        // API服务
        private readonly IImagePlanService ImageService;
        private readonly ITextPlanService TextService;
        // 属性内部字段
        private ObservableCollection<PlanBase> plans; // 要展示的计划表
        private string rightEditorTitle; // 右侧编辑栏的标题（主要用来区别是新增还是修改）
        private string rightEditorButton; // 右侧编辑栏的按钮
        private bool isRightTextEditorOpen; // 右侧编辑窗弹出情况 - 文本编辑页
        private bool isRightImageEditorOpen; // 右侧编辑窗弹出情况 - 图像编辑页
        private string searchText; // 所搜索的文本，单向到源绑定
        private PlanBase currentEditPlan; // 当前在编辑栏中编辑的对象，双向绑定
        // 本地字段
        private bool isAddOrModify; // 0 为 Add, 1 为 Modify
        #endregion

        #region 属性
        // 命令绑定
        public DelegateCommand<string> EditOfAddPlanCommand { get; private set; } // 增加命令（不知道这种能不能合并到Exec中，待修改）
        public DelegateCommand<PlanBase> EditOfModifyPlanCommand { get; private set; } // 修改命令
        public DelegateCommand<PlanBase> DeletePlanCommand { get; private set; } // 删除命令
        public DelegateCommand<PlanBase> CompletePlanCommand { get; private set; } // 完成命令

        public DelegateCommand<string> ExecCommand { get; private set; } // 执行无参数命令（有参数的因为CommandParameter要绑定参数，无法绑定命令，只能用另一种）
        public DelegateCommand SearchPlanCommand { get; private set; } // 查找命令
        public DelegateCommand UpdatePlanCommand { get; private set; } // 上传更新命令
        // 外部访问属性
        /// <summary>
        /// 所有的计划列表
        /// </summary>
        public ObservableCollection<PlanBase> Plans { get { return plans; } private set { plans = value; RaisePropertyChanged(); } }
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
        public string SearchText { get { return searchText; } set { searchText = value; } }
        /// <summary>
        /// 所选择的将要进行修改的计划，双向绑定，展示到文本栏同步更改
        /// </summary>
        public PlanBase CurrentEditPlan { get { return currentEditPlan; } set { currentEditPlan = value; RaisePropertyChanged(); } }
        #endregion

        /// <summary>
        /// Init - 构造函数，只执行一次
        /// </summary>
        /// <param name="imageService">图片类计划的服务</param>
        /// <param name="textService">文本类计划的服务</param>
        /// <param name="containerProvider">该页面容器</param>
        public PlanViewModel(IImagePlanService imageService, ITextPlanService textService, IContainerProvider containerProvider) : base(containerProvider)
        {
            // 内部服务初始化
            ImageService = imageService;
            TextService = textService;
            // 部分初始展示属性初始化
            Plans = new ObservableCollection<PlanBase>();
            IsRightTextEditorOpen = false;
            IsRightImageEditorOpen = false;
            searchText = null;
            // 各种命令的初始化
            EditOfAddPlanCommand = new DelegateCommand<string>(EditOfAddPlan);
            EditOfModifyPlanCommand = new DelegateCommand<PlanBase>(EditOfModifyPlan);
            DeletePlanCommand = new DelegateCommand<PlanBase>(DeletePlan);
            CompletePlanCommand = new DelegateCommand<PlanBase>(CompletePlan);
            ExecCommand = new DelegateCommand<string>(Exec);
            SearchPlanCommand = new DelegateCommand(SearchPlan);
            UpdatePlanCommand = new DelegateCommand(UpdatePlan);
        }

        // 已验证√
        /// <summary>
        /// 增加计划
        /// </summary>
        /// <param name="type">传的参数，用"Text"表示增加的文本类，"Image"表示增加的图片类</param>
        private void EditOfAddPlan(string type)
        {
            Loading(true);
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
                IsRightImageEditorOpen = true; // 窗口为“图片编辑状态”
                RightEditorTitle = "人形计划添加";
                RightEditorButton = "确认添加";
                CurrentEditPlan = new ImagePlanClass(0, false, null);
            }
            Loading(false);
        }

        // 已验证√
        /// <summary>
        /// 修改计划（会赋值给右侧编辑栏的用于相应属性SelectUpdatePlan，用于更改）
        /// </summary>
        /// <param name="obj">修改计划的基类</param>
        public void EditOfModifyPlan(PlanBase obj) // TODO: 0 - obj传来的是什么需要确认
        {
            Loading(true);
            isAddOrModify = false; // 操作是修改

            if (obj is TextPlanClass) // 选中的文本类消息
            {
                IsRightTextEditorOpen = true; // 窗口为“文本编辑状态”
                RightEditorTitle = "常规计划修改";
                RightEditorButton = "确认修改";
                TextPlanClass t = (TextPlanClass)obj;
                CurrentEditPlan = new TextPlanClass(t.ID, t.Status, t.Title, t.Content);
            }
            else // 选中的图片类消息
            {
                IsRightImageEditorOpen = true; // 窗口为“图片编辑状态”
                RightEditorTitle = "人形计划修改";
                RightEditorButton = "确认修改";
                ImagePlanClass t = (ImagePlanClass)obj;
                CurrentEditPlan = new ImagePlanClass(t.ID, t.Status, t.TDoll_ID);
            }
            Loading(false);
        }

        // 已验证√
        /// <summary>
        /// 删除计划
        /// </summary>
        /// <param name="detelePlan">所删除计划的基类</param>
        public async void DeletePlan(PlanBase detelePlan)
        {
            try
            {
                // TODO: 3 - 添加温馨提示
                //var dialogResult = await dialogHost.Question("温馨提示", $"确认删除待办事项:{obj.Title} ?");
                //if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                Loading(true);

                var deleteResponse = detelePlan.Type == PlanBase.PlanType.Text ? await TextService.Delete(detelePlan.ID) : await ImageService.Delete(detelePlan.ID);
                if (deleteResponse.Status == APIStatusCode.Success)
                    Plans.Remove(detelePlan);
                else
                    throw new Exception("删除出错啦，肯定不是服务器的问题qwq！……");
            }
            finally
            {
                Loading(false);
            }
        }

        // 已验证√
        /// <summary>
        /// 完成任务的命令
        /// </summary>
        /// <param name="planBase">所完成的任务，传的为基类</param>
        public async void CompletePlan(PlanBase planBase)
        {
            APIStatusCode APIResponseStatus;
            try
            {
                Loading(true);
                if (planBase is TextPlanClass) // 基类是TextPlan类型
                {
                    TextPlanClass plan = (TextPlanClass)planBase;
                    plan.Status = true;
                    APIResponseStatus = (await TextService.Update(plan.ConvertDTO(plan, 1))).Status;
                }
                else // 基类是ImagePlan类型
                {
                    ImagePlanClass plan = (ImagePlanClass)planBase;
                    plan.Status = true;
                    APIResponseStatus = (await ImageService.Update(plan.ConvertDTO(plan, 1))).Status;
                }
                if (APIResponseStatus == APIStatusCode.Success)
                    planBase.Status = true; // 将本地的状态也更改
                else
                    throw new Exception("诶，好像改不了这个任务的状态，待会再试试呢【……");
            }
            finally
            {
                Loading(false);
            }
        }

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
            finally { }
        }

        /// <summary>
        /// 查询该用户包含条件的计划，目前只支持文字类计划的搜索
        /// </summary>
        private async void SearchPlan()
        {
            try
            {
                Loading(true);
                Plans.Clear();

                if (SearchText == null)
                    GetAllPlansForUserAsyna();
                else
                {
                    var textPlanResult = await TextService.GetParamContainForUser(new GETParameter() { user_id = 1, search = SearchText, field = "Title" }); // TODO: 2 - 这里选的是查标题
                    if (textPlanResult.Status != APIStatusCode.Success)
                        throw new Exception("服务器查询出错了，是不是被铁血打进来了呢x……");
                    foreach (var textItem in textPlanResult.Result.Items)
                        Plans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));
                }
            }
            finally
            {
                Loading(false);
            }
        }

        // 已验证√
        // TODO: 3 - 小问题：新增的在最后而不是最前
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
                    Loading(true);
                    APIStatusCode updateResponseStatus = isAddOrModify ? (await TextService.Add(textPlan.ConvertDTO(textPlan, 1))).Status : (await TextService.Update(textPlan.ConvertDTO(textPlan, 1))).Status;
                    if (updateResponseStatus == APIStatusCode.Success)
                    {
                        if (isAddOrModify) // 代表新增
                            Plans.Add(textPlan);
                        else
                        {
                            int index;
                            for (index = 0; index <= Plans.Count; index++)
                                if (Plans[index] is TextPlanClass && Plans[index].ID == textPlan.ID) // 通过遍历ID找到所修改的数据
                                    break;
                            if (index <= Plans.Count)
                                Plans[index] = textPlan;
                            else
                                throw new Exception("内部错误 - 修改计划后，遍历的index无法找到对应的数据……");
                        }
                    }
                    IsRightTextEditorOpen = false; // 关闭编辑页
                }
                else
                {
                    ImagePlanClass imagePlan = (ImagePlanClass)CurrentEditPlan;
                    if (imagePlan.TDoll_ID < 0 || imagePlan.TDoll_ID > TDollsContext.MaxTDoll_ID) // 人形计划的人形ID不满足范围
                        throw new Exception("输入的战术人形ID不存在啦，请检查一下呢……"); // 返回错误提示
                    Loading(true);
                    APIStatusCode updateResponseStatus = isAddOrModify ? (await ImageService.Add(imagePlan.ConvertDTO(imagePlan, 1))).Status : (await ImageService.Update(imagePlan.ConvertDTO(imagePlan, 1))).Status;
                    if (updateResponseStatus == APIStatusCode.Success)
                    {
                        if (isAddOrModify) // 代表新增
                            Plans.Add(imagePlan);
                        else
                        {
                            int index;
                            for (index = 0; index <= Plans.Count; index++)
                                if (Plans[index] is ImagePlanClass && Plans[index].ID == imagePlan.ID) // 通过遍历ID找到所修改的数据
                                    break;
                            if (index <= Plans.Count) // 代表修改
                                Plans[index] = imagePlan;
                            else
                                throw new Exception("内部错误 - 修改计划后，遍历的index无法找到对应的数据……");
                        }
                    }
                    IsRightImageEditorOpen = false; // 关闭编辑页
                }
            }
            catch (Exception ex)
            {
                // TODO: 2 - 这里增加弹窗报错
            }
            finally
            {
                Loading(false);
            }
        }

        /// <summary>
        /// 查询该用户所有计划
        /// </summary>
        async void GetAllPlansForUserAsyna()
        {
            try
            {
                Loading(true); // 弹出等待会话
                Plans.Clear();

                // TODO: 2 - 警告，这里给的用户id指定为给1
                // TODO: 3 - 这里可以只改成加载一次，用flag标记
                var imagePlanResult = await ImageService.GetAllForUser(1); // 通过服务，查询数据库ImagePlan中所有元组。
                var textPlanResult = await TextService.GetAllForUser(1);
                // 以下按照创建时间降序展现
                if (imagePlanResult.Status != APIStatusCode.Success)
                    throw new Exception("嘘！服务器在睡觉，不要打扰她啦……");
                if (imagePlanResult != null)
                {
                    int imageIndex = 0, textIndex = 0;
                    while (imageIndex < imagePlanResult.Result.Items.Count && textIndex < textPlanResult.Result.Items.Count)
                    {
                        var imageItem = imagePlanResult.Result.Items[imageIndex];
                        var textItem = textPlanResult.Result.Items[textIndex];
                        if (imageItem.CreateDate > textItem.CreateDate)
                        { Plans.Add(new ImagePlanClass(imageItem.ID, imageItem.Status, imageItem.TDoll_ID)); imageIndex++; }
                        else
                        { Plans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content)); textIndex++; }
                    }
                    for (; imageIndex < imagePlanResult.Result.Items.Count; imageIndex++)
                    {
                        var imageItem = imagePlanResult.Result.Items[imageIndex]; Plans.Add(new ImagePlanClass(imageItem.ID, imageItem.Status, imageItem.TDoll_ID));
                    }
                    for (; textIndex < textPlanResult.Result.Items.Count; textIndex++)
                    {
                        //var textItem = textPlanResult.Result.Items[textIndex]; Plans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));
                    }
                }
            }
            finally
            {
                Loading(false);
            }
        }

        /// <summary>
        /// 重写导航加载到该页面的方法
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetAllPlansForUserAsyna(); // 来到该页面时，默认重新读取用户所有的数据
        }

    }
}