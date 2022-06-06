﻿using CourseDesign.Common.Classes;
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

namespace CourseDesign.ViewModels
{
    internal class PlanViewModel : NavigationViewModel
    {
        #region 字段
        private ObservableCollection<PlanBase> plans;
        private bool isRightDrawerOpen;
        private string searchText;

        // 命令绑定
        public DelegateCommand<string> ExecCommand { get; private set; }
        public DelegateCommand AddPlanCommand { get; private set; }
        // API服务
        private readonly IImagePlanService ImageService;
        private readonly ITextPlanService TextService;
        #endregion

        #region 属性
        /// <summary>
        /// 搜索文本，双向绑定
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; }
        }
        /// <summary>
        /// 右侧编辑窗是否弹出
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 所有的计划列表
        /// </summary>
        public ObservableCollection<PlanBase> Plans
        {
            get { return plans; }
            set { plans = value; RaisePropertyChanged(); }
        }
        #endregion

        /// <summary>
        /// init构造函数，初始化
        /// </summary>
        /// <param name="imageService">图片类计划的服务</param>
        /// <param name="textService">文本类计划的服务</param>
        /// <param name="containerProvider">该页面容器</param>
        public PlanViewModel(IImagePlanService imageService, ITextPlanService textService, IContainerProvider containerProvider) : base(containerProvider)
        {
            Plans = new ObservableCollection<PlanBase>();
            // 各种命令的初始化
            ExecCommand = new DelegateCommand<string>(Exec);
            AddPlanCommand = new DelegateCommand(AddPlan);

            ImageService = imageService;
            TextService = textService;
        }

        private void Exec(string cmd)
        {
            switch (cmd)
            {
                case "新增": AddPlan(); break;
                case "修改": UpdatePlan(); break;
                case "查询": QueryPlan(); break;
            }
        }

/* 新增部分 */
        private PlansBase modifyPlan;
        public PlansBase ModifyPlan =
        {
            get { return modifyPlan; }
            set { modifyPlan = value; RaisePropertyChanged(); }
        }
        // 修改计划
        private async void modifyPlan(PlanBase obj)
        {
            try
            {
                Loading(ture);

                if (obj.type = PlansBase.TypeEnum.Text) // 文本类消息
                {
                    var textPlanResult = await textService.GetID(obj.ID); // 这里还未实现
                    modifyPlan = textPlanResult.Result;
                }
                else
                {
                    var imagePlanResult = await imageService.GetID(obj.ID);
                    modifyPlan = imagePlanResult.Result;
                }

                Loading(false);   
            }
        }
        // 添加计划
        privare async void addPlan()
        {
            if (string.IsNullOrWhiteSpace(obj.Title) || string.IsNullOrWhitSpace(obj.Content))
                rertun; // 返回错误提示
            Loading(true);
            var addResult = await textService.Add(currentPlan)
            if (addResult.Status)
            {
                Plan.Add(addResult.Result);
            }
                IsRightDrawerOpen=false;   
        }
        // 更新文本计划
        private async void UpdatePlan()
        {
            if (string.IsNullOrWhiteSpace(currentPlan.Title) || string.IsNullOrWhitSpace(currentPlan.Content))
                rertun; // 返回错误提示
            Loading(true);
            var updateResult = await textService.Update(currentPlan)
            if (UpdateResult.Status)
            {
                var plan=currentPlan.FIrstOrDefault(t => t.ID == currentPlan.ID)
                if (plan != null)
                {
                    plan.Title = currentPlan.Title;
                    plan.Content = currentPlan.Content;
                    plan.Status = currentPlan.Status;
                }
            }
                IsRightDrawerOpen=false; 
        }
        /// <summary>
        /// 右侧编辑窗是否弹出，这里可能要根据选择的是image还是text，返回不同的
        /// </summary>
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 所有的计划列表
        /// </summary>
        public ObservableCollection<PlansBase> Plans
        {
            get { return tasks; }
            set { tasks = value; RaisePropertyChanged(); }
        }



        /// <summary>
        /// 增加计划
        /// </summary>
        private void AddPlan()
        {
            IsRightDrawerOpen = true;
        }
        /// <summary>
        /// 修改计划
        /// </summary>
        private void UpdatePlan()
        {

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
