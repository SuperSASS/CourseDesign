using CourseDesign.Command.Classes;
using CourseDesign.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace CourseDesign.ViewModels
{
    internal class PlanViewModel : BindableBase
    {
        public DelegateCommand AddPlanCommand { get; private set; }
        private bool isRightDrawerOpen;
        private ObservableCollection<PlansBase> tasks;
        private readonly IImagePlanService ImageService;

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
        public ObservableCollection<PlansBase> Plans
        {
            get { return tasks; }
            set { tasks = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// init构造函数，初始化
        /// </summary>
        /// <param name="imageService"></param>
        public PlanViewModel(IImagePlanService imageService)
        {
            Plans = new ObservableCollection<PlanBase>();
            // 各种命令的初始化
            ExecCommand = new DelegateCommand<string>(Exec);
            AddPlanCommand = new DelegateCommand(AddPlan);
            // API服务初始化
            ImageService = imageService;
            LoadPlansForUser();
        }

        /// <summary>
        /// 增加计划
        /// </summary>
        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        /// <summary>
        /// 查询该用户包含筛选关键词的计划
        /// </summary>
        private async void QueryPlan()
        {
            Plans.Clear();
            // TODO: 3 - 警告，这里没有加外键约束，所有人都可以访问到所有Plan
            var imagePlanResult = await ImageService.GetAllForUser(new QueryParameter()); // 通过服务，查询数据库ImagePlan中所有元组，最多查询前100项（分页大小为100）

            //var textPlanResult = await TextService.GetAllForUser(1);
            var textPlanResult = await TextService.GetParamContainForUser(1, new QueryParameter() { search = SearchText, field = "Title" }); // TODO: 2 - 这里默认搜索的是标题
            foreach (var textItem in textPlanResult.Result.Items)
                Plans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));

            Loading(false);
        }

        /// <summary>
        /// 查询该用户所有计划
        /// </summary>
        async void GetPlansForUserAsyna()
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
                    Plans.Add(new ImagePlansClass(item.ID, item.Status, item.TDoll_ID));
                }
            }
        }

        // 以下为测试样例
        //int image_ID = 0, text_ID = 0;
        //for (int i=1; i<=10; i++)
        //{
        //    if (i % 2 == 0) // 用偶数来模拟是文字类计划 
        //    {
        //        Plans.Add(new TextPlansClass(++text_ID, false, "文字类计划测试", "内容这里是！……"));
        //        Plans.Add(new TextPlansClass(++text_ID, false, "文字类计划测试", "长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容长内容"));
        //    }
        //    else
        //        Plans.Add(new ImagePlansClass(++image_ID, false, 1));
        //}
    }
}
