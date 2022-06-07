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

namespace CourseDesign.ViewModels
{
    internal class PlanViewModel : NavigationViewModel
    {
        #region 字段
        // API服务
        private readonly IImagePlanService ImageService;
        private readonly ITextPlanService TextService;
        // 属性内部字段
        private ObservableCollection<PlanBase> plans; // 要展示的计划表
        private string rightEditerTitle; // 右侧编辑栏的标题（主要用来区别是新增还是修改）
        private string rightEditerButton; // 右侧编辑栏的按钮
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
        public DelegateCommand<PlanBase> CompletePlanCommand { get; private set;}

        public DelegateCommand<string> ExecCommand { get; private set; } // 执行无参数命令（有参数的因为CommandParameter要绑定参数，无法绑定命令，只能用另一种）
        public DelegateCommand SearchPlanCommand { get; private set; } // 查找命令
        public DelegateCommand UpdatePlanCommand { get; private set; } // 上传更新命令
        public DelegateCommand CompletePlanCommand {get; private set;} // 完成计划命令
        // 外部访问属性
        /// <summary>
        /// 所有的计划列表
        /// </summary>
        public ObservableCollection<PlanBase> Plans { get { return plans; } private set { plans = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑栏的标题
        /// </summary>
        public string RightEditerTitle { get { return rightEditerTitle; } private set { rightEditerTitle = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑栏的按钮
        /// </summary>
        public string RightEditerButton { get { return rightEditerButton; } private set { rightEditerButton = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑窗弹出情况，0为不弹出，1为弹出文本类编辑栏，2为弹出图片类编辑栏
        /// </summary>
        public int IsRightTextEditorOpen { get { return isRightTextEditorOpen; } private set { isRightTextEditorOpen = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑窗弹出情况，0为不弹出，1为弹出文本类编辑栏，2为弹出图片类编辑栏
        /// </summary>
        public int IsRightImageEditorOpen { get { return isRightImageEditorOpen ; } private set { isRightImageEditorOpen = value; RaisePropertyChanged(); } }
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
            IsRightImageEditerOpen = false;
            searchText = null;
            // 各种命令的初始化
            EditOfAddPlanCommand = new DelegateCommand<string>(EditOfAddPlan)
            EditOfModifyPlanCommand = new DelegateCommand<PlanBase>(EditOfModifyPlan);
            DeletePlanCommand = new DeletePlanCommand<PlanBase>(DeletePlan);
            CompletePlanCommand = new DelegateCommand<PlanBase>(CompletePlan);
            ExecCommand = new DelegateCommand<string>(Exec);
            SearchPlanCommand = new DelegateCommand(SearchPlan);
            UpdatePlanCommand = new DelegateCommand(UpdatePlan);
        }

        private void EditOfAddPlan(string type)
        {
            try
            {
                Loading(true);
                isAddOrModify = false; // 操作是增加

                if (type.Equal("Text")) // 选中的文本类消息
                {
                    IsRightTextEditorOpen = true; // 窗口为“文本编辑状态”
                    rightEditerTitle = "常规计划添加";
                    rightEditerButton = "确认添加";
                    CurrentEditPlan = new TextPlanClass();
                }
                else // 选中的图片类消息
                {
                    IsRightImageEditorOpen = true; // 窗口为“图片编辑状态”
                    rightEditerTitle = "人形计划添加";
                    rightEditerButton = "确认添加";
                    var t = (ImagePlanClass)obj;
                    CurrentEditPlan = new ImagePlanClass();
                }
            }
            catch (Exception ex) // 未处理异常情况
            {
            }
            finally
            {
                Loading(false);
            }
        }

        // 选择修改。会赋值给右侧编辑栏的用于相应属性SelectUpdatePlan，用于更改。
        private async void EditOfModifyPlan(PlanBase obj) // TODO: 0 - obj传来的是什么需要确认
        {
            try
            {
                Loading(true);
                isAddOrModify = true; // 操作是修改

                if (obj.Type == PlanBase.PlanType.Text) // 选中的文本类消息
                {
                    IsRightTextEditorOpen = true; // 窗口为“文本编辑状态”
                    rightEditerTitle = "常规计划修改";
                    rightEditerButton = "确认修改";
                    CurrentEditPlan = new TextPlanClass(obj.ID, obj.Status, obj.Title, obj.Content);
                }
                else // 选中的图片类消息
                {
                    IsRightImageEditorOpen = true; // 窗口为“图片编辑状态”
                    rightEditerTitle = "人形计划修改";
                    rightEditerButton = "确认修改";
                    // var imageServiceResopnseResult = (await ImageService.GetID(obj.ID)).Result;
                    CurrentEditPlan = new ImagePlanClass(obj.ID, obj.Status, obj.TDoll_ID);
                }
            }
            catch (Exception ex) // 未处理异常情况
            {
            }
            finally
            {
                Loading(false);
            }
        }

        // 删除
        private async void DeletePlan(PlanBase detelePlan)
        {
            try
            {
                // TODO: 3 - 添加温馨提示
                //var dialogResult = await dialogHost.Question("温馨提示", $"确认删除待办事项:{obj.Title} ?");
                //if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                Loading(true);

                var deleteResponse = detelePlan.Type == PlanBase.PlanType.Text ? await TextService.Delete(detelePlan.ID) : await ImageService.Delete(detelePlan.ID);
                if (deleteResult.Status == APIStatusCode.Success)
                    Plans.Remove(Plans.FirstOrDefault(t => t.ID==detelePlan.ID));
            }
            finally
            {
                Loading(false);
            }
        }


        private void CompletePlanCommand(PlanBase completePlan)
        {
            Loadinging(true);
            var plan = Plans.FirstOrDefault(t => t.ID==completePlan.ID);
            plan.Status = true;
            var completeResponse = completePlan.Type=PlanBase.PlanType.Text ? textService.Update(plan) : imageService.Update(plan);
            if (completeResponse.Status != APIStatusCode.Success)
                Plans.Status = false; // 回滚事务
        }

        /// <summary>
        /// 命令执行的总命令
        /// </summary>
        /// <param name="cmd">执行的命令</param>
        private void Exec(string cmd)
        {
            switch (cmd)
            {
                // case "新增": EditOfAddPlan(param); break;
                // case "修改": EditOfModifyPlan((PlanBase)param); break;
                // case "完成": CompletePlan(); break;
                // case "删除": DeletePlan(); break;
                case "查询": SearchPlan(); break;
                case "上传": UpdatePlan(); break;
            }
        }


        /// <summary>
        /// 查询该用户包含条件的计划，目前只支持文字内计划的搜索
        /// </summary>
        private async void SearchPlan()
        {
            Loading(true);
            Plans.Clear();

            if (SearchText == null)
                GetAllPlansForUserAsyna();
            else
            {
                var textPlanResult = await TextService.GetParamContainForUser(new GETParameter() { user_id = 1, search = SearchText, field = "Title" }); // TODO: 2 - 这里选的是查标题
                foreach (var textItem in textPlanResult.Result.Items)
                    Plans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));
            }
            Loading(false);
        }

        // 更新计划
        private async void UpdatePlan()
        {
            if (IsRightTextEditorOpen) // 打开的是文字编辑
            {
                if (string.IsNullOrWhiteSpace(CurrentEditPlan.Title) || string.IsNullOrWhitSpace(currentEditPlan.Content)) // 计划的标题或内容为空
                rertun; // 返回错误提示
            Loading(true);
            var updateResult = await isAddOrModify ? textService.Add(currentEditPlan) : textService.Update(currentEditPlan);
            if (UpdateResult.Status)
            {
                var plan = Plan.FirstOrDefault(t => t.ID == currentEditPlan.ID);
                if (plan != null)
                {
                    plan.Title = currentEditPlan.Title;
                    plan.Content = currentEditPlan.Content;
                    plan.Status = currentEditPlan.Status;
                }
                else // 代表新增
                    plan.Add(currentEditPlan);
            }
            IsRightTextEditerOpen = false;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(currentEditPlan.TDoll_ID)) // 人形计划的人形ID为空
                rertun; // 返回错误提示
            Loading(true);
            var updateResult = await isAddOrModify ? textService.Add(currentEditPlan) : textService.Update(currentEditPlan);
            if (UpdateResult.Status)
            {
                var plan = Plan.FirstOrDefault(t => t.ID == currentEditPlan.ID);
                if (plan != null)
                    plan.TDoll_ID = currentEditPlan.Title;
                else // 代表新增
                    plan.Add(currentEditPlan);
            }
            IsRightImageEditerOpen = false;
            }

        }

        /// <summary>
        /// 查询该用户所有计划
        /// </summary>
        async void GetAllPlansForUserAsyna()
        {
            Loading(true); // 弹出等待会话
            Plans.Clear();

            // TODO: 2 - 警告，这里给的用户id指定为给1
            // TODO: 3 - 这里可以只改成加载一次，用flag标记
            var imagePlanResult = await ImageService.GetAllForUser(1); // 通过服务，查询数据库ImagePlan中所有元组。
            var textPlanResult = await TextService.GetAllForUser(1);

            if (imagePlanResult != null && imagePlanResult.Status == APIStatusCode.Success)
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
                    var textItem = textPlanResult.Result.Items[textIndex]; Plans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));
                }
            }
            Loading(false);
        }

        /// <summary>
        /// 重写导航加载到该页面的方法
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            GetAllPlansForUserAsyna();
        }
    }
}
