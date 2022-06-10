using CourseDesign.Common.Classes;
using CourseDesign.Services.Interfaces;
using CourseDesign.Shared;
using CourseDesign.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseDesign.ViewModels;

namespace CourseDesign.Context
{
    /// <summary>
    /// 有关用户的全局上下文
    /// </summary>
    public class LoginUserContext
    {
        private static int loginUserID;
        private static List<PlanBase> userPlans;
        private static List<int> userTDolls;

        // API服务
        private readonly IImagePlanService ImageService;
        private readonly ITextPlanService TextService;
        private readonly ITDollService TDollService;

        #region 属性
        /// <summary>
        /// 当前登录的用户ID
        /// </summary>
        public static int LoginUserID
        {
            get { return loginUserID; }
            private set { loginUserID = value; }
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
            set { userTDolls = value; }
        }
        #endregion

        #region 方法


        public LoginUserContext(int loginUserID, IImagePlanService imagePlanService, ITextPlanService textPlanService, ITDollService tDollService)
        {
            LoginUserID = loginUserID;
            ImageService = imagePlanService;
            TextService = textPlanService;
            TDollService = tDollService;

            UserPlans = new List<PlanBase>();
            UserTDolls = new List<int>();
            FirstLoadUserContext();
        }

        /// <summary>
        /// 第一次加载该用户的各项数据
        /// </summary>
        async void FirstLoadUserContext()
        {
            UserTDolls.Clear();
            UserPlans.Clear();

            try
            {
                /* 加载用户所拥有人形 */
                var tDollsResult = await TDollService.GetUserAndParamContain(new GETParameter() { user_id = LoginUserID });
                if (tDollsResult == null || tDollsResult.Status != APIStatusCode.Success)
                    throw new Exception("嘘！服务器在睡觉，不要打扰她啦……");
                foreach (var item in tDollsResult.Result.Items)
                    UserTDolls.Add(item.ID);
                /* 加载用户所制定的计划 */
                var imagePlanResult = await ImageService.GetAllForUser(LoginUserID); // 通过服务，查询数据库ImagePlan中所有元组。
                var textPlanResult = await TextService.GetAllForUser(LoginUserID);
                // 以下按照创建时间降序展现
                if (imagePlanResult == null || textPlanResult == null || imagePlanResult.Status != APIStatusCode.Success || textPlanResult.Status != APIStatusCode.Success)
                    throw new Exception("嘘！服务器在睡觉，不要打扰她啦……");

                int imageIndex = 0, textIndex = 0;
                while (imageIndex < imagePlanResult.Result.Items.Count && textIndex < textPlanResult.Result.Items.Count)
                {
                    var imageItem = imagePlanResult.Result.Items[imageIndex];
                    var textItem = textPlanResult.Result.Items[textIndex];
                    if (imageItem.CreateDate > textItem.CreateDate)
                    { UserPlans.Add(new ImagePlanClass(imageItem.ID, imageItem.Status, imageItem.TDoll_ID)); imageIndex++; }
                    else
                    { UserPlans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content)); textIndex++; }
                }
                for (; imageIndex < imagePlanResult.Result.Items.Count; imageIndex++)
                {
                    var imageItem = imagePlanResult.Result.Items[imageIndex];
                    UserPlans.Add(new ImagePlanClass(imageItem.ID, imageItem.Status, imageItem.TDoll_ID));
                }
                for (; textIndex < textPlanResult.Result.Items.Count; textIndex++)
                {
                    var textItem = textPlanResult.Result.Items[textIndex];
                    UserPlans.Add(new TextPlanClass(textItem.ID, textItem.Status, textItem.Title, textItem.Content));
                }
            }
            catch
            {
            }
        }

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
