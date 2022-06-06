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
        private string rightEditerTitle; // 右侧编辑栏的标题
        private string rightEditerButton; // 右侧编辑栏的按钮
        private int statusOfRightEditerOpen; // 右侧图像计划编辑栏开启情况
        private string searchText; // 所搜索的文本，单向到源绑定
        private PlanBase currentEditPlan; // 当前在编辑栏中编辑的对象，双向绑定
        #endregion

        #region 属性
        // 命令绑定
        public DelegateCommand<string> ExecCommand { get; private set; } // 执行各种命令
        public DelegateCommand<PlanBase> SelectAddPlanCommand { get; private set; } // 增加增加命令
        public DelegateCommand<PlanBase> SelectModifyPlanCommand { get; private set; } // 增加增加命令
        public DelegateCommand DeletePlanCommand { get; private set; } // 增加删除命令
        public DelegateCommand SearchPlanCommand { get; private set; } // 增加查询命令
        public DelegateCommand UpdatePlanCommand { get; private set; } // 增加修改命令
        // 外部访问属性
        /// <summary>
        /// 所有的计划列表
        /// </summary>
        public ObservableCollection<PlanBase> Plans { get { return plans; } set { plans = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑栏的标题
        /// </summary>
        public string RightEditerTitle { get { return rightEditerTitle; } set { rightEditerTitle = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑栏的按钮
        /// </summary>
        public string RightEditerButton { get { return rightEditerButton; } set { rightEditerButton = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 右侧编辑窗弹出情况，0为不弹出，1为弹出文本类编辑栏，2为弹出图片类编辑栏
        /// </summary>
        public int StatusOfRightEditerOpen { get { return statusOfRightEditerOpen; } set { statusOfRightEditerOpen = value; RaisePropertyChanged(); } }
        /// <summary>
        /// 搜索文本，单向到源绑定
        /// </summary>
        public string SearchText { get { return searchText; } set { searchText = value; } }
        /// <summary>
        /// 所选择的将要进行修改的计划，双向绑定
        /// </summary>
        public PlanBase CurrentEditPlan { get { return currentEditPlan; } set { currentEditPlan = value; RaisePropertyChanged(); } }
        #endregion

        /// <summary>
        /// Init - 构造函数
        /// </summary>
        /// <param name="imageService">图片类计划的服务</param>
        /// <param name="textService">文本类计划的服务</param>
        /// <param name="containerProvider">该页面容器</param>
        public PlanViewModel(IImagePlanService imageService, ITextPlanService textService, IContainerProvider containerProvider) : base(containerProvider)
        {
            // 内部服务初始化
            ImageService = imageService;
            TextService = textService;
            // 属性初始化
            Plans = new ObservableCollection<PlanBase>();
            IsRightImageEditerOpen = false;
            SearchText = string.Empty;
            // 各种命令的初始化
            ExecCommand = new DelegateCommand<string>(Exec);
            SelectAddPlanCommand = new DelegateCommand(SelectAddPlan);
            SelectModifyPlanCommand = new DelegateCommand<PlanBase>(EditOfModifyPlan);
            DeletePlanCommand = new DelegateCommand(DeletePlan);
            SearchPlanCommand = new DelegateCommand(SearchPlan);
            UpdatePlanCommand = new DelegateCommand(UpdatePlan);
        }

        /// <summary>
        /// 命令执行的总命令
        /// </summary>
        /// <param name="cmd">执行的命令</param>
        private void Exec(string cmd, PlanBase obj = null)
        {
            switch (cmd)
            {
                case "新增": EditOfAddPlan(obj); break;
                case "修改": EditOfModifyPlan(obj); break;
                case "删除": DeletePlan(); break;
                case "查询": QueryPlan(); break;
                case "上传": UpdatePlan(); break;
            }
        }

        private void EditOfAddPlan(PlanBase obj)
        {
            throw new NotImplementedException();
        }

        // 选择修改。会赋值给右侧编辑栏的用于相应属性SelectUpdatePlan，用于更改
        private async void EditOfModifyPlan(PlanBase obj)
        {
            try
            {
                Loading(true);

                if (obj.Type == PlanBase.PlanType.Text) // 选中的文本类消息
                {
                    var textServiceResopnseResult = (await TextService.GetID(obj.ID)).Result; // 因为都能显示出来了，肯定能GetID到的，这里不做判断直接用Result
                    CurrentEditPlan = new TextPlanClass(textServiceResopnseResult.ID, textServiceResopnseResult.Status, textServiceResopnseResult.Title, textServiceResopnseResult.Content);
                }
                else // 选中的图片类消息
                {
                    var imageServiceResopnseResult = (await ImageService.GetID(obj.ID)).Result;
                    CurrentEditPlan = new ImagePlanClass(imageServiceResopnseResult.ID, imageServiceResopnseResult.Status, imageServiceResopnseResult.TDoll_ID);
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
        // 添加计划
        private async void addPlan()
        {
            //if (string.IsNullOrWhiteSpace(obj.Title) || string.IsNullOrWhitSpace(obj.Content))
            //    rertun; // 返回错误提示
            //Loading(true);
            //var addResult = await textService.Add(currentPlan)
            //if (addResult.Status)
            //{
            //    Plan.Add(addResult.Result);
            //}
            //    IsRightDrawerOpen=false;   
        }
        // 更新文本计划
        private async void UpdatePlan()
        {
            if (string.IsNullOrWhiteSpace(CurrentEditPlan.Title) || string.IsNullOrWhitSpace(currentEditPlan.Content))
                rertun; // 返回错误提示
            Loading(true);
            var updateResult = await textService.Update(currentEditPlan);
            if (UpdateResult.Status)
            {
                var plan = currentEditPlan.FIrstOrDefault(t => t.ID == currentEditPlan.ID);
                if (plan != null)
                {
                    plan.Title = currentEditPlan.Title;
                    plan.Content = currentEditPlan.Content;
                    plan.Status = currentEditPlan.Status;
                }
            }
            IsRightImageEditerOpen = false;
        }


        /// <summary>
        /// 增加计划
        /// </summary>
        private void AddPlan()
        {
            IsRightImageEditerOpen = true;
        }

        /// <summary>
        /// 查询该用户包含条件的计划
        /// </summary>
        private async void QueryPlan()
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
