using CourseDesign.Common.Classes;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseDesign.ViewModels;
using CourseDesign.Services.API.Interfaces;
using CourseDesign.Common.Classes.Bases;

namespace CourseDesign.Context
{
    /// <summary>
    /// 有关用户的全局上下文，只需要ID便可确定所有数据
    /// </summary>
    public class LoginUserContext
    {
        #region 字段
        // API服务
        private readonly IImagePlanService ImageService;
        private readonly ITextPlanService TextService;
        private readonly ITDollService TDollService;
        // 字段
        private static UserClass loginUser;
        // 导出字段（由loginUser所确定的数据）
        private static List<PlanBase> userPlans;
        private static List<int> userTDolls;
        private static int userPlansComplete;
        #endregion

        #region 静态属性
        /// <summary>
        /// 当前登录的用户
        /// </summary>
        public static UserClass LoginUser
        {
            get { return loginUser; }
            private set { loginUser = value; }
        }
        /// <summary>
        /// 当前用户所拥有所有计划
        /// </summary>
        public static List<PlanBase> UserPlans
        {
            get { return userPlans; }
            private set { userPlans = value; }
        }
        /// <summary>
        /// 当前用户所拥有的所有人形
        /// </summary>
        public static List<int> UserTDolls
        {
            get { return userTDolls; }
            private set { userTDolls = value; }
        }
        /// <summary>
        /// 用户计划完成数【这里直接把修改权限暴露出来了，可能应该改为一个用方法更改的，像LoginUser那样
        /// </summary>
        public static int UserPlansComplete
        {
            get { return userPlansComplete; }
            set { userPlansComplete = value; }
        }
        #endregion

        #region 初始化
        public LoginUserContext(UserClass loginUser, IImagePlanService imagePlanService, ITextPlanService textPlanService, ITDollService tDollService)
        {
            // 属性赋值
            LoginUser = loginUser;
            // 服务赋值
            ImageService = imagePlanService;
            TextService = textPlanService;
            TDollService = tDollService;
            // 用户上下文初始化（复位）
            UserPlans = new();
            UserTDolls = new();
            userPlansComplete = 0;
            // 加载用户上下文（先创建任务等待队列）
            AddLoginUserWaitTasks();
        }

        /// <summary>
        /// 增加生成用户上下文所需的等待任务
        /// </summary>
        private void AddLoginUserWaitTasks()
        {
            ContextWaitTasks.WaitTasks.Add(Task.Run(FirstLoadUserContext));
        }

        /// <summary>
        /// 第一次加载该用户的各项数据
        /// </summary>
        private async Task FirstLoadUserContext()
        {
            try
            {
                /* 加载用户所拥有人形 */
                var tDollsResult = await TDollService.GetUserAndParamContain(new GETParameter() { user_id = LoginUser.UserID });
                if (tDollsResult.Status != APIStatusCode.Success)
                    throw new Exception(tDollsResult.Message);
                foreach (var item in tDollsResult.Result.Items)
                    UserTDolls.Add(item.ID);
                /* 加载用户所制定的计划 */
                var imagePlanResult = await ImageService.GetAllForUser(LoginUser.UserID); // 通过服务，查询数据库ImagePlan中所有元组。
                if (imagePlanResult.Status != APIStatusCode.Success)
                    throw new Exception(imagePlanResult.Message);
                var textPlanResult = await TextService.GetAllForUser(LoginUser.UserID);
                if (textPlanResult.Status != APIStatusCode.Success)
                    throw new Exception(textPlanResult.Message);
                /* 完成对API的访问，以下按照创建时间降序展现 */
                int imageIndex = 0, textIndex = 0;
                while (imageIndex < imagePlanResult.Result.Items.Count && textIndex < textPlanResult.Result.Items.Count)
                {
                    var imageItem = imagePlanResult.Result.Items[imageIndex];
                    var textItem = textPlanResult.Result.Items[textIndex];
                    if (imageItem.CreateDate > textItem.CreateDate)
                    {
                        UserPlans.Add(new ImagePlanClass(imageItem.ID, imageItem.Status, imageItem.TDoll_ID));
                        imageIndex++;
                        if (imageItem.Status) UserPlansComplete++;
                    }
                    else
                    {
                        UserPlans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));
                        textIndex++;
                        if (textItem.Status) UserPlansComplete++;
                    }
                }
                for (; imageIndex < imagePlanResult.Result.Items.Count; imageIndex++)
                {
                    var imageItem = imagePlanResult.Result.Items[imageIndex];
                    UserPlans.Add(new ImagePlanClass(imageItem.ID, imageItem.Status, imageItem.TDoll_ID));
                    if (imageItem.Status) UserPlansComplete++;
                }
                for (; textIndex < textPlanResult.Result.Items.Count; textIndex++)
                {
                    var textItem = textPlanResult.Result.Items[textIndex];
                    UserPlans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));
                    if (textItem.Status) UserPlansComplete++;
                }
            }
            // 这里的任务完成状态，将由WaitTasks里的IsFaulted等属性呈现
            finally
            {
            }
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 在静态的全局用户上下文中修改用户数据
        /// </summary>
        /// <param name="user">修改后的用户数据，类型为<see cref="UserClass"/></param>
        public static void ChangeUserInfo(UserClass user) {LoginUser = user; }

        /// <summary>
        /// 得到计划数据，用于修改本地数据的状态
        /// </summary>
        /// <param name="ID">计划ID</param>
        /// <returns><see cref="PlanBase"/>类型的两种计划数据</returns>
        public static PlanBase GetPlan(int ID)
        {
            foreach (var item in userPlans)
                if (item.ID == ID)
                    return item;
            return null;
        }
        #endregion
    }
}
