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
            Plans = new ObservableCollection<PlansBase>();
            AddPlanCommand = new DelegateCommand(Add);
            //CreatePlans();
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

        async void LoadPlansForUser()
        {
            Plans.Clear();
            // TODO: 3 - 警告，这里没有加外键约束，所有人都可以访问到所有Plan
            var imagePlanResult = await ImageService.GetAllForUser(new QueryParameter()); // 通过服务，查询数据库ImagePlan中所有元组，最多查询前100项（分页大小为100）

            if (imagePlanResult.Status == false)
            {
                foreach (var item in imagePlanResult.Result.Items)
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
