﻿using CourseDesign.Common.Classes;
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
        private static int loginUserID;
        private static List<PlanBase> userPlans;
        private static List<int> userTDolls;
        private static List<Task> waitTasks;
        private static int userPlansComplete;
        #endregion

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
            private set { userTDolls = value; }
        }
        /// <summary>
        /// 所需要等待的任务组
        /// </summary>
        public static List<Task> WaitTasks
        {
            get { return waitTasks; }
            set { waitTasks = value; }
        }
        /// <summary>
        /// 用户计划完成数
        /// </summary>
        public static int UserPlansComplete
        {
            get { return userPlansComplete; }
            set { userPlansComplete = value; }
        }
        #endregion

        #region 方法
        public LoginUserContext(int loginUserID, IImagePlanService imagePlanService, ITextPlanService textPlanService, ITDollService tDollService)
        {
            // 字段和服务赋值
            LoginUserID = loginUserID;
            ImageService = imagePlanService;
            TextService = textPlanService;
            TDollService = tDollService;
            // 用户上下文初始化
            UserPlans = new();
            UserTDolls = new();
            WaitTasks = new();
            InitializeWaitTasks();
        }

        async void InitializeWaitTasks()
        {
            WaitTasks.Add(FirstLoadUserContext());
            foreach (var waitTask in WaitTasks)
                await waitTask;
        }

        /// <summary>
        /// 第一次加载该用户的各项数据
        /// </summary>
        private async Task FirstLoadUserContext()
        {
            UserTDolls.Clear();
            UserPlans.Clear();
            try
            {
                /* 加载用户所拥有人形 */
                var tDollsResult = await TDollService.GetUserAndParamContain(new GETParameter() { user_id = LoginUserID });
                if (tDollsResult.Status != APIStatusCode.Success)
                    throw new Exception(tDollsResult.Message);
                foreach (var item in tDollsResult.Result.Items)
                    UserTDolls.Add(item.ID);
                /* 加载用户所制定的计划 */
                var imagePlanResult = await ImageService.GetAllForUser(LoginUserID); // 通过服务，查询数据库ImagePlan中所有元组。
                if (imagePlanResult.Status != APIStatusCode.Success)
                    throw new Exception(imagePlanResult.Message);
                var textPlanResult = await TextService.GetAllForUser(LoginUserID);
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
            catch (Exception ex)
            {
            }
            finally
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
