using CourseDesign.Command.Classes;
using CourseDesign.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.DTOs;
using CourseDesign.Shared.Parameters;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using CourseDesign.Context;

namespace CourseDesign.ViewModels
{
    internal class PlanViewModel : BindableBase
    {
        public DelegateCommand AddPlanCommand { get; private set; }
        private bool isRightDrawerOpen;
        private ObservableCollection<PlanBase> plans;
        // API服务
        private readonly IImagePlanService ImageService;
        private readonly ITextPlanService TextService;

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

        /// <summary>
        /// init构造函数，初始化
        /// </summary>
        /// <param name="imageService"></param>
        public PlanViewModel(IImagePlanService imageService, ITextPlanService textService)
        {
            Plans = new ObservableCollection<PlanBase>();
            AddPlanCommand = new DelegateCommand(Add);
            ImageService = imageService;
            TextService = textService;
            // 读取数据
            FirstLoadPlansForUser();
        }

        /// <summary>
        /// 增加计划
        /// </summary>
        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        async void FirstLoadPlansForUser()
        {
            Plans.Clear();
            // TODO: 2 - 警告，这里给的用户id指定为给1
            var imagePlanResult = await ImageService.GetAllForUser(1); // 通过服务，查询数据库ImagePlan中所有元组。
            var textPlanResult = await TextService.GetAllForUserAsync(1);

            if (imagePlanResult != null && imagePlanResult.Status == APIStatusCode.Success)
            {
                int imageIndex = 0, textIndex = 0;
                while (imageIndex <= imagePlanResult.Result.Items.Count && textIndex <= textPlanResult.Result.Items.Count)
                {
                    var imageItem = imagePlanResult.Result.Items[imageIndex];
                    var textItem = textPlanResult.Result.Items[textIndex];
                    if (imageItem.ID < textItem.ID)
                    { Plans.Add(new ImagePlanClass(imageItem.ID, imageItem.Status, imageItem.TDoll_ID)); imageIndex++; }
                    else
                    { Plans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content)); textIndex++; }
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
