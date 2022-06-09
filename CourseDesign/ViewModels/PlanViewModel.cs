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
using System.Collections.Generic;
using System.Windows.Controls;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;

namespace CourseDesign.ViewModels
{
    public class PlanViewModel : NavigationViewModel
    {
        #region 字段
        // API服务
        private readonly IImagePlanService ImageService;
        private readonly ITextPlanService TextService;
        // 属性内部字段
        private List<PlanBase> plansInner; // 所存储的所有计划内容，在状态筛选之后的
        private ObservableCollection<PlanBase> plansShow; // 要展示的计划表
        private string rightEditorTitle; // 右侧编辑栏的标题（主要用来区别是新增还是修改）
        private string rightEditorButton; // 右侧编辑栏的按钮
        private bool isRightTextEditorOpen; // 右侧编辑窗弹出情况 - 文本编辑页
        private bool isRightImageEditorOpen; // 右侧编辑窗弹出情况 - 图像编辑页
        private string searchContentText; // 所搜索的文本，单向到源绑定
        private ComboBoxItem searchFieldText; // 所搜索的字段，单向绑定到源
        private ComboBoxItem searchStatus; // 所选择展示的计划状态，单项绑定到源
        private PlanBase currentEditPlan; // 当前在编辑栏中编辑的对象，双向绑定
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
        public ObservableCollection<PlanBase> PlansShow { get { return plansShow; } private set { plansShow = value; RaisePropertyChanged(); } }
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
        public string SearchContentText { get { return searchContentText; } set { searchContentText = value; } }

        private bool isFirstCreate_SFT = true; // 加这个临时字段，防止第一次加载时，计划数据被重复读取（会调用SearchFiledText、SearchStatus和初始化各一次，又因为异步可能不会等全部清楚后再添加
        /// <summary>
        /// 搜索字段，单项绑定到源
        /// </summary>
        public ComboBoxItem SearchFieldText
        {
            get { return searchFieldText; }
            set { searchFieldText = value; if (isFirstCreate_SFT) isFirstCreate_SFT = false; else SearchPlan(); }
        }

        private bool isFirstCreate_SS = true; // 加这个临时字段，防止第一次加载时，计划数据被重复读取（会调用SearchFiledText、SearchStatus和初始化各一次，又因为异步可能不会等全部清楚后再添加
        /// <summary>
        /// 筛选的状态，单向绑定到源
        /// </summary>
        public ComboBoxItem SearchStatus
        {
            get { return searchStatus; }
            set { searchStatus = value; if (isFirstCreate_SS) isFirstCreate_SS = false; else SearchPlan(); }
        }
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
            plansInner = new List<PlanBase>();
            plansShow = new ObservableCollection<PlanBase>();
            IsRightTextEditorOpen = false;
            IsRightImageEditorOpen = false;
            searchContentText = null;
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
        /// <param name="deletePlan">所删除计划的基类</param>
        public async void DeletePlan(PlanBase deletePlan)
        {
            try
            {
                // TODO: 3 - 添加温馨提示
                //var dialogResult = await dialogHost.Question("温馨提示", $"确认删除待办事项:{obj.Title} ?");
                //if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                Loading(true);

                var deleteResponse = deletePlan.Type == PlanBase.PlanType.Text ? await TextService.Delete(deletePlan.ID) : await ImageService.Delete(deletePlan.ID);
                if (deleteResponse.Status == APIStatusCode.Success)
                { plansInner.Remove(deletePlan); PlansShow.Remove(deletePlan); }
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
        /// 对文字类计划，切换任务完成状态的命令
        /// </summary>
        /// <param name="textPlan">所完成的任务，传的为基类</param>
        public async void ChangeCompleteForTextPlan(TextPlanClass textPlan)
        {
            APIStatusCode APIResponseStatus;
            try
            {
                Loading(true);
                TextPlanClass plan = (TextPlanClass)textPlan;
                plan.Status = textPlan.Status;
                APIResponseStatus = (await TextService.Update(plan.ConvertDTO(plan, 1))).Status;
                if (APIResponseStatus == APIStatusCode.Success)
                    textPlan.Status = textPlan.Status; // 将本地的状态也更改
                else
                    throw new Exception("诶，好像改不了这个任务的状态，待会再试试呢【……");
                CreateShowPlans(); // 重新生成显示内容
            }
            finally
            {
                Loading(false);
            }
        }

        /// <summary>
        /// 对图片类计划，只能完成任务的命令
        /// </summary>
        public async void CompleteForImagePlan(ImagePlanClass imagePlan)
        {
            APIStatusCode APIResponseStatus;
            try
            {
                Loading(true);
                ImagePlanClass plan = (ImagePlanClass)imagePlan;
                plan.Status = true; // 对于图片类计划，只能完成，所以要单独提出来orz……
                APIResponseStatus = (await ImageService.Update(plan.ConvertDTO(plan, 1))).Status;
                if (APIResponseStatus == APIStatusCode.Success)
                    imagePlan.Status = imagePlan.Status; // 将本地的状态也更改
                else
                    throw new Exception("诶，好像改不了这个任务的状态，待会再试试呢【……");
                CreateShowPlans(); // 重新生成显示内容
            }
            finally
            {
                Loading(false);
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
            finally { }
        }

        /// <summary>
        /// 查询该用户包含条件的计划，目前只支持文字类计划的标题包含搜索
        /// </summary>
        private async void SearchPlan()
        {
            try
            {
                Loading(true);
                plansInner.Clear();
                PlansShow.Clear();
                // 生成plansInner
                if (string.IsNullOrWhiteSpace(SearchContentText))
                    GetAllPlansForUserAndCreateShowAsyna();
                else
                {
                    GETParameter t = new GETParameter()
                    {
                        user_id = 1,
                        search = SearchContentText,
                        field = ConvertComboBoxItemToField(SearchFieldText)
                    };
                    var textPlanResult = await TextService.GetParamContainForUser(new GETParameter()
                    {
                        user_id = 1,
                        search = SearchContentText,
                        field = ConvertComboBoxItemToField(SearchFieldText)
                    }); // TODO: 2 - 这里选的是查标题
                    if (textPlanResult.Status != APIStatusCode.Success)
                        throw new Exception("服务器查询出错了，是不是被铁血打进来了呢x……");
                    foreach (var textItem in textPlanResult.Result.Items)
                        plansInner.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));
                    CreateShowPlans();
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
                        { plansInner.Add(textPlan); PlansShow.Add(textPlan); }
                        else
                        {
                            int index;
                            for (index = 0; index <= PlansShow.Count; index++)
                                if (PlansShow[index] is TextPlanClass && PlansShow[index].ID == textPlan.ID) // 通过遍历ID找到所修改的数据
                                    break;
                            if (index <= PlansShow.Count)
                                PlansShow[index] = textPlan;
                            else
                                throw new Exception("内部错误 - 修改计划后，遍历的index无法找到对应的数据……");
                        }
                    }
                    IsRightTextEditorOpen = false; // 关闭编辑页
                }
                else
                {
                    ImagePlanClass imagePlan = (ImagePlanClass)CurrentEditPlan;
                    imagePlan.TDoll = TDollsContext.GetTDoll((int)imagePlan.TDoll_ID); // 要得到这个计划的人形信息，用于添加后展示
                    if (imagePlan.TDoll_ID < 0 || imagePlan.TDoll_ID > TDollsContext.MaxTDoll_ID) // 人形计划的人形ID不满足范围
                        throw new Exception("输入的战术人形ID不存在啦，请检查一下呢……"); // 返回错误提示
                    Loading(true);
                    APIResponse<ImagePlanDTO> updateResponse = isAddOrModify ? await ImageService.Add(imagePlan.ConvertDTO(imagePlan, 1)) : await ImageService.Update(imagePlan.ConvertDTO(imagePlan, 1));
                    if (updateResponse.Status == APIStatusCode.Success)
                    {
                        if (isAddOrModify) // 代表新增【Fix：新增上传后，要把服务器里的ID加到imagePlan里，否则在立刻删除时会因为没有ID而报错……
                        { imagePlan.ID = updateResponse.Result.ID; plansInner.Add(imagePlan); PlansShow.Add(imagePlan); }
                        else
                        {
                            int index;
                            for (index = 0; index <= plansInner.Count; index++)
                                if (plansInner[index] is ImagePlanClass && plansInner[index].ID == imagePlan.ID) // 通过遍历ID找到所修改的数据
                                    break;
                            if (index <= plansInner.Count) // 代表修改
                            { plansInner[index] = imagePlan; CreateShowPlans(); } // 由于不知道为什么直接修改不会通知更新，所以这里只能先删后加来更新了orz…… //Plans[index] = imagePlan;
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
        /// 重写导航加载到该页面的方法
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetAllPlansForUserAndCreateShowAsyna(); // 来到该页面时，默认重新读取用户所有的数据
        }

        #region 内部方法

        /// <summary>
        /// 先查询该用户所有计划，放到plans_inner里；再根据选择的任务状态进行筛选展示
        /// </summary>
        async void GetAllPlansForUserAndCreateShowAsyna()
        {
            try
            {
                Loading(true); // 弹出等待会话
                plansInner.Clear();

                // 生成读取的所有计划（不考虑完成状态）
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
                        { plansInner.Add(new ImagePlanClass(imageItem.ID, imageItem.Status, imageItem.TDoll_ID)); imageIndex++; }
                        else
                        { plansInner.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content)); textIndex++; }
                    }
                    for (; imageIndex < imagePlanResult.Result.Items.Count; imageIndex++)
                    {
                        var imageItem = imagePlanResult.Result.Items[imageIndex]; plansInner.Add(new ImagePlanClass(imageItem.ID, imageItem.Status, imageItem.TDoll_ID));
                    }
                    for (; textIndex < textPlanResult.Result.Items.Count; textIndex++)
                    {
                        var textItem = textPlanResult.Result.Items[textIndex]; plansInner.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));
                    }
                }
                CreateShowPlans();

            }
            finally
            {
                Loading(false);
            }
        }

        /// <summary>
        /// 生成用于展示的计划
        /// </summary>
        void CreateShowPlans()
        {
            PlansShow.Clear();
            foreach (PlanBase item in plansInner)
                if (SearchStatus == null
                    || (SearchStatus.Content.Equals("未完成") && item.Status == false)
                    || (SearchStatus.Content.Equals("已完成") && item.Status == true))
                    PlansShow.Add(item);
        }

        /// <summary>
        /// 用于将view中筛选栏的选项，转换为DTO总的Field
        /// </summary>
        /// <param name="item">筛选栏的选项</param>
        /// <returns>DTO中的Field</returns>
        string ConvertComboBoxItemToField(ComboBoxItem item)
        {
            if (item == null)
                return null;
            switch (item.Content.ToString())
            {
                case "计划标题": return "Title";
                case "计划内容": return "Content";
                default: return null;
            }
        }

        #endregion
    }
}